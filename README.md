# sharp-render

A terminal-based BMP image viewer that renders bitmap images directly in your console using ANSI 256-color codes.

## Overview

sharp-render is a C# application that parses BMP files and displays them in the terminal by converting pixels to colored block characters. It automatically scales images to fit your terminal window while preserving as much detail as possible through bilinear interpolation.

## Example

![A side by side comparison](images/sidebyside.png)

## Features

- **BMP File Parsing**: Reads uncompressed 24-bit BMP files
- **Automatic Scaling**: Resizes images to fit your terminal dimensions using bilinear interpolation
- **Color Mapping**: Intelligently maps millions of colors to the 256-color ANSI palette
- **Cross-Platform**: Works on any terminal that supports ANSI escape codes

## Requirements

- .NET 9.0 or later
- A terminal that supports ANSI 256-color escape codes (most modern terminals, but not windows cmd)

## Installation
  
Precombiled binaries are avaliable on the releases.
  
To compile:
```bash
git clone https://github.com/Username0103/sharp-render.git
cd sharp-render
dotnet build
```

## Usage

For best results, size your terminal to be close to the aspect ratio of the target image.  
Decreasing the font size increases the avaliable resolution, but it does get a lot slower.
  
Example commands:
```bash
dotnet run -h
dotnet run "C:\path\to\your\image.bmp"
```

## Limitations

- Only supports uncompressed BMP format
- Requires 24-bit color depth
- Image quality depends on terminal size and font

## Future Enhancements

- Support for additional image formats (PNG, JPEG)
- Different scaling algorithms
- ASCII art mode for non-color terminals

## License

MIT Liscense

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
