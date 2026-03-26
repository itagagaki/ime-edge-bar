Add-Type -AssemblyName System.Drawing

function New-IcoLayer([int]$n) {
    $bmp = New-Object System.Drawing.Bitmap($n, $n, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g   = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.Clear([System.Drawing.Color]::Transparent)

    # Bar occupies the middle 50%; 25% transparent margins top and bottom
    $top  = [int][Math]::Round($n * 0.25)
    $bot  = $n - $top
    $barH = $bot - $top

    # Green background bar
    $bg = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::Green)
    $g.FillRectangle($bg, 0, $top, $n, $barH)
    $bg.Dispose()

    # White downward-pointing arrow filling the bar cross-section
    $half = [int]($barH / 2)
    $cx   = [int]($n / 2)
    $pts  = [System.Drawing.Point[]]@(
        [System.Drawing.Point]::new($cx - $half, $top),
        [System.Drawing.Point]::new($cx + $half, $top),
        [System.Drawing.Point]::new($cx, $bot)
    )
    $fg = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::White)
    $g.FillPolygon($fg, $pts)
    $fg.Dispose()

    $g.Dispose()
    return $bmp
}

# Render each size to a PNG byte array
$sizes = @(16, 32, 48, 256)
$png   = @{}
foreach ($s in $sizes) {
    $bmp = New-IcoLayer $s
    $ms  = New-Object System.IO.MemoryStream
    $bmp.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
    $png[$s] = $ms.ToArray()
    $ms.Dispose()
    $bmp.Dispose()
}

# Write ICO file  (header + directory entries + PNG image data)
$out = Join-Path $PSScriptRoot 'app.ico'
$fs  = [System.IO.File]::Create($out)
$bw  = New-Object System.IO.BinaryWriter($fs)

# ICO header (6 bytes)
$bw.Write([uint16]0)             # reserved
$bw.Write([uint16]1)             # type: icon
$bw.Write([uint16]$sizes.Count)  # image count

# Compute image data offsets
$off     = 6 + 16 * $sizes.Count
$offsets = @()
foreach ($s in $sizes) { $offsets += $off; $off += $png[$s].Length }

# Directory entries (16 bytes each)
for ($i = 0; $i -lt $sizes.Count; $i++) {
    $s  = $sizes[$i]
    $wh = if ($s -eq 256) { [byte]0 } else { [byte]$s }
    $bw.Write($wh)                        # width  (0 = 256)
    $bw.Write($wh)                        # height (0 = 256)
    $bw.Write([byte]0)                    # color count
    $bw.Write([byte]0)                    # reserved
    $bw.Write([uint16]1)                  # planes
    $bw.Write([uint16]32)                 # bit count
    $bw.Write([uint32]$png[$s].Length)    # bytes in resource
    $bw.Write([uint32]$offsets[$i])       # offset to image data
}

# PNG image data
foreach ($s in $sizes) { $bw.Write($png[$s]) }

$bw.Flush()
$bw.Dispose()
$fs.Dispose()
Write-Host "Created: $out"
