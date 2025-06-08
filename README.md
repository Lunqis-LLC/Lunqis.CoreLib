# Lunqis.CoreLib

| | [中文](README_CN.md) | |

Lunqis.CoreLib is a .NET Standard 2.1 class library providing a set of core utilities and abstractions for building robust, extensible, and maintainable applications. The library includes:

- **Background Task Framework**: Interfaces and base classes (`ITask`, `IStartTask`, `BackGroundTask`, `ScheduledTask`, `TimerTask`) for defining, scheduling, and executing background and scheduled tasks with retry and error handling support.
- **Pipeline Middleware**: Abstractions for building middleware pipelines, enabling modular request/response processing (see `IPipeline`, `IMiddleware`, and related classes).
- **Extension Methods**: A rich set of extension methods for `string`, `MethodInfo`, `ParameterInfo`, and collections, including type conversions, file operations, cryptographic hashing, and more.
- **Async Resource Management**: Utilities like `ListAsyncDisposable<T>` for managing collections of asynchronously disposable resources.
- **State Machine Parsers**: Tools for parsing command-line arguments and text blocks using state machine patterns (`ArgsParse`, `BlockParse`).

The library is designed for flexibility and can be integrated into a wide range of .NET applications, including services, tools, and middleware-based architectures.

Licensed under the MIT License.