# Lunqis.CoreLib

Lunqis.CoreLib 是一个基于 .NET Standard 2.1 的类库，提供了一套用于构建健壮、可扩展和易维护应用程序的核心工具和抽象。该库包括：

- **后台任务框架**：用于定义、调度和执行后台及定时任务的接口与基类（`ITask`、`IStartTask`、`BackGroundTask`、`ScheduledTask`、`TimerTask`），支持重试与错误处理。
- **管道中间件**：用于构建中间件管道的抽象，支持模块化的请求/响应处理（参见 `IPipeline`、`IMiddleware` 及相关类）。
- **扩展方法**：为 `string`、`MethodInfo`、`ParameterInfo` 及集合等类型提供丰富的扩展方法，包括类型转换、文件操作、加密哈希等功能。
- **异步资源管理**：如 `ListAsyncDisposable<T>`，用于管理异步可释放资源的集合。
- **状态机解析器**：使用状态机模式解析命令行参数和文本块的工具（`ArgsParse`、`BlockParse`）。

该库设计灵活，可集成到各种 .NET 应用程序中，包括服务、工具和基于中间件的架构。

本项目基于 MIT 许可证发布。