# Hermes 交接记录

> 面向后续协作 AI 的阶段性事实记录。新阶段请在文末追加，不覆盖旧记录。

## 2026-06-27｜第一阶段：可运行 Web API 骨架与客户/供应商模块

### 当前完成

- 从空目录建立 `LogisticsInvoice.sln` 和 `src/LogisticsInvoice.Api`。
- 项目目标框架为 `.NET 6`（`net6.0`）。
- 已配置 Swagger、EF Core、SQL Server LocalDB 和依赖注入。
- 已实现统一响应 `ApiResponse<T>`、分页结果 `PagedResult<T>`。
- 已实现全局异常中间件和统一参数校验错误。
- 已实现 `GET /api/health`，同时检查 API 和数据库连接。
- 已实现客户/供应商新增、修改、分页筛选、启用/停用。
- 已创建 README、学习疑问记录、交接记录、面试讲法。

### 当前项目结构

```text
src/LogisticsInvoice.Api
├─ Controllers
├─ Services
├─ Repositories
├─ Entities
├─ Dtos/BusinessParties
├─ Common
├─ Infrastructure/Data
└─ Infrastructure/Middleware
```

核心建模决定：客户和供应商统一为 `BusinessParty`，用 `BusinessPartyType` 的 `Customer`、`Supplier`、`Both` 区分。

本机环境决定：使用已经安装的 SQL Server LocalDB。`global.json` 固定 SDK 8.0.422 编译 `net6.0`，应用仍运行在 .NET 6 Runtime。EF Core 使用兼容 .NET 6 的 7.0.20。

### 尚未完成

- EF Core Migration；第一阶段为了零配置启动使用 `EnsureCreated`。
- 发票主表、发票明细和状态流转。
- JWT 登录、用户、角色与接口授权。
- 操作日志。
- 报表统计。
- Excel 导入导出。
- 单元测试、集成测试、部署说明。

### 暴露或推测需要补强的知识点

当前没有用户显式提问，以下是根据转型目标推测的学习重点，不应表述为用户已经不会：

- .NET SDK、Target Framework 和 Runtime 的区别。
- ASP.NET Core Middleware 与依赖注入生命周期。
- Entity 与 DTO 的边界。
- EF Core 跟踪查询、`AsNoTracking`、异步查询、Migration。
- HTTP 状态码、统一业务响应和异常处理如何配合。
- Repository 是否必要，避免为了分层而分层。

### 下一步建议

1. 先让用户亲自通过 Swagger 完成一次新增、查询、修改、停用，并复述调用链。
2. 加入 EF Core Migration，把数据库结构纳入版本管理。
3. 设计发票主表、明细、金额字段和状态机，再实现发票 CRUD。
4. 在状态流转稳定后加入 JWT 和角色授权。
5. 最后补操作日志、报表与 Excel，避免同时铺太多模块。

### 当前可写进简历的内容

- 基于 ASP.NET Core Web API、EF Core 和 SQL Server 构建物流发票管理学习项目。
- 采用 Controller、Service、Repository 的清晰单体分层，完成客户/供应商主数据的新增、修改、条件分页和状态管理。
- 设计统一 API 响应、参数校验和全局异常处理，使用 Swagger 完成接口调试与文档展示。

### 不能夸大的风险点

- 这是学习项目，尚未上线生产，也没有真实并发量和用户规模。
- 目前只有客户/供应商模块，不能描述为“已完成物流发票完整系统”。
- 尚未完成 JWT、权限、操作日志、发票、报表、Excel 和自动化测试。
- `EnsureCreated` 是第一阶段开发便利方案，不代表生产数据库变更实践。
- 可以关联真实 ERP/发票/报表维护经验，但不能把本项目说成原公司的生产系统或商业成果。

### 第一阶段验收证据

- `dotnet build LogisticsInvoice.sln --no-restore`：构建成功，0 error；存在预期的 `NETSDK1138` 警告，因为 .NET 6 已结束官方支持。
- Swagger JSON：`GET /swagger/v1/swagger.json` 返回 200，包含 `/api/business-parties`。
- 健康检查：返回 `Healthy` 和 `Database=Connected`。
- 真实调用链已通过：新增得到 ID 1；关键词分页命中 1 条；修改类型为 `Both`；停用返回成功。
- 统一错误已验证：修改不存在 ID 返回 HTTP 404 和 `ApiResponse`；空编码、空名称返回 HTTP 400 和统一校验错误。
