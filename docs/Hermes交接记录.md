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

## 2026-06-28｜数据库切换：SQL Server LocalDB → Oracle Database XE

### 当前完成

- 数据库 Provider 从 `Microsoft.EntityFrameworkCore.SqlServer` 替换为 Oracle 官方 `Oracle.EntityFrameworkCore 7.21.13`。
- `Program.cs` 从 `UseSqlServer` 切换为 `UseOracle`。
- 连接字符串改为 Oracle Easy Connect：`localhost:1521/XEPDB1`。
- 新增 `docker-compose.yml`，使用本机已有的 Oracle Database XE 21c 官方镜像、数据卷和幂等 Schema 启动脚本。
- 应用使用 `logistics_invoice` Schema，不使用 `SYS`/`SYSTEM` 保存业务表。
- README、学习疑问和面试讲法已同步更新为 Oracle 方案。

### 技术取舍

- Oracle 更贴近用户过去的 ERP、发票、报表和 Oracle 维护经历，面试叙事更连贯。
- 本机没有 Oracle 服务或客户端，因此使用 Docker，避免在 Windows 直接安装完整数据库。
- ODP.NET 是纯托管驱动，API 宿主机不需要安装 Oracle Client。
- 当前使用 `container-registry.oracle.com/database/express:21.3.0-xe`，属于 Oracle Container Registry 的官方 XE 镜像。
- 仓库中的密码仅用于可丢弃的本地开发容器，真实环境必须安全覆盖。

### 当前项目结构变化

```text
LogisticsInvoice.Api/
├─ docker-compose.yml
├─ src/LogisticsInvoice.Api/
│  ├─ Program.cs                  # UseOracle
│  ├─ appsettings.json            # Oracle Easy Connect
│  └─ Infrastructure/Data/
└─ docs/
```

### 下一步建议

1. 让用户理解 Oracle 的 CDB、PDB、Service Name、User 和 Schema。
2. 用 Oracle EF Core Migration 替换 `EnsureCreated`。
3. 在发票金额设计前确认 `NUMBER(p,s)` 精度及 .NET `decimal` 映射。
4. 后续如使用原生 SQL，必须单独验证 Oracle 方言，不能假设 SQL Server SQL 可直接迁移。

### 可写进简历的更新

- 使用 Oracle 官方 ODP.NET EF Core Provider 接入 Oracle Database XE，并通过 Docker Compose 构建可复现的本地数据库环境。
- 将应用数据放在独立 Oracle Schema 中，完成健康检查和客户/供应商 CRUD 的真实数据库验证。

### 不能夸大的风险点

- 目前只验证简单 CRUD、筛选和分页，尚未覆盖复杂 Oracle SQL、PL/SQL、存储过程或性能调优。
- Docker 本地环境不等于生产 Oracle 部署经验。
- Provider 切换的代码量较小，不应表述成完成了大型异构数据库迁移。

### 2026-06-29 实际验收证据

- Oracle 容器 `logistics-invoice-oracle` 状态为 `healthy`。
- SQL*Plus 使用 `logistics_invoice/LogisticsDev123@XEPDB1` 登录成功。
- EF Core 在 `LOGISTICS_INVOICE` Schema 中实际创建了 `BusinessParties` 表。
- Swagger JSON 返回 200，健康检查返回 `Healthy` 和 `Database=Connected`。
- Oracle 上真实执行新增、关键词分页、修改为 `Both`、停用和不存在 ID 的 404，全部通过。
- SQL*Plus 直接查到验收记录为 `Type=Both`、`IsEnabled=0`；验收后记录已删除，表内剩余 0 行。
- 容器再次重启后仍为 `healthy`，幂等脚本没有重复创建用户，应用 Schema 和 `BusinessParties` 表均保留。

## 2026-06-29｜进入用户动手练习模式

- 新增 `docs/学习任务.md`。
- 当前只布置“学习任务 1：按 ID 查询客户/供应商”。
- `IBusinessPartyService` 已给出方法契约。
- `BusinessPartyService.GetByIdAsync` 和 `BusinessPartiesController.GetById` 中的 `TODO(学习任务 1)` 是故意保留给用户完成的，不是待助手自动修复的缺陷。
- 用户完成后应先评分和给出证据，不要直接覆盖其代码。
