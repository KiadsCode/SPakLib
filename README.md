# SPackLib

**SPackLib** — lightweight and efficient shader packaging library for FNA-based projects.  
Supports multiple compression formats including default GZip, alternative 7-bit encoding, and advanced UCSP (Ultra Compressed Shader Pack) with LUT + DEFLATE compression.

---

## Features

- Pack and unpack compiled shader bytecode efficiently
- Multiple compression formats for size/performance tradeoffs
- Easy integration with FNA and other .NET game frameworks
- Designed for small-to-medium indie projects
- Extensible format interface for custom packaging

---

## Installation

Clone the repository or download the latest release from GitHub.  
Add the `SPackLib` project or DLL reference to your solution.

---

## License

MIT License

Copyright (c) 2025 iwnddm

Permission is hereby granted, free of charge, to any person obtaining a copy  
of this software and associated documentation files (the "Software"), to deal  
in the Software without restriction, including without limitation the rights  
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell  
copies of the Software, and to permit persons to whom the Software is  
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all  
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR  
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,  
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE  
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER  
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,  
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE  
SOFTWARE.

---

## Author Notice

You are free to use, modify, and distribute this software in your own projects,  
provided that you keep the copyright notice intact and acknowledge the original author.

Original author: iwnddm

---

## Roadmap

- Add support for additional compression algorithms
- Implement CLI tool for shader packaging
- Create WPF GUI for easy packaging/unpacking
- Optimize compression and decompression speed further
- Expand documentation and usage examples
- Consider integration with other game engines
