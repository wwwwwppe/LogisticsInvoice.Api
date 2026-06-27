# LogisticsInvoice.Api

一个面向求职转型与面试展示的物流发票、客户/供应商和报表管理后端。项目来自真实 ERP、发票、对账和报表维护经验的业务抽象，目标是在不过度设计的前提下，练习现代 ASP.NET Core Web API 的常用能力。

> 当前为第一阶段：先把可运行、可调用、可讲清楚的单体 API 骨架搭起来，再逐步加入发票、状态流转、权限、日志和报表。

## 技术栈

- .NET 6（项目目标框架 `net6.0`）
- ASP.NET Core Web API
- Entity Framework Core 7.0.20
- SQL Server LocalDB
- Swagger / OpenAPI
- 依赖注入、DTO 参数校验
- 全局异常处理中间件
- 统一响应结构 `ApiResponse<T>`

### 为什么这样选择

- **SQL Server LocalDB**：当前 Windows 开发机已经安装，可直接运行，不需要 Docker、账号或额外数据库安装。
- **EF Core 7**：仍支持 `net6.0`，并使用较新的 SQL Server 驱动依赖；项目运行目标仍是 .NET 6。
- **单体分层**：Controller、Service、Repository 职责清楚，足够支撑学习和面试展示，又没有引入微服务、DDD、CQRS 等当前不必要的复杂度。
- **`EnsureCreated` 自动建库**：第一阶段降低启动门槛。正式迭代会切换为 EF Core Migration，以便管理数据库版本变化。

## 当前完成功能

- Swagger API 文档
- `GET /api/health` 服务与数据库健康检查
- 客户/供应商新增
- 客户/供应商修改
- 客户/供应商分页查询，可按关键词、类型、启用状态筛选
- 客户/供应商启用/停用
- DTO 数据校验及统一校验错误响应
- 业务异常与未知异常的全局处理
- SQL Server LocalDB 自动建库

客户和供应商共用 `BusinessParty`（业务往来单位）模型，通过 `Type` 区分：

- `Customer`：客户
- `Supplier`：供应商
- `Both`：既是客户也是供应商

## 项目结构

```text
LogisticsInvoice.Api/
├─ src/LogisticsInvoice.Api/
│  ├─ Controllers/       # HTTP 接口与状态码
│  ├─ Services/          # 业务规则与流程编排
│  ├─ Repositories/      # EF Core 数据访问
│  ├─ Entities/          # 数据库实体
│  ├─ Dtos/              # 接口输入输出模型
│  ├─ Common/            # 统一响应、分页、业务异常
│  └─ Infrastructure/    # DbContext、全局中间件等基础设施
└─ docs/                 # 学习、交接与面试材料
```

## 如何启动

### 环境要求

- Windows
- .NET 6 SDK，或能构建 `net6.0` 的更高版本 SDK
- SQL Server LocalDB

本仓库的 `global.json` 使用本机已有的 .NET SDK 8.0.422 构建 `net6.0`。应用实际运行仍使用 .NET 6 运行时。

### 启动命令

```powershell
sqllocaldb start MSSQLLocalDB
dotnet restore
dotnet run --project .\src\LogisticsInvoice.Api
```

浏览器打开：

```text
https://localhost:7080/swagger
```

健康检查：

```text
GET https://localhost:7080/api/health
```

第一次启动时，EF Core 会自动创建本地数据库 `LogisticsInvoiceDb`。

## 接口一览

| 方法 | 地址 | 说明 |
|---|---|---|
| GET | `/api/health` | 服务和数据库健康检查 |
| POST | `/api/business-parties` | 新增客户/供应商 |
| PUT | `/api/business-parties/{id}` | 修改客户/供应商 |
| GET | `/api/business-parties` | 分页及条件查询 |
| PATCH | `/api/business-parties/{id}/status` | 启用或停用 |

新增示例：

```json
{
  "code": "CUS001",
  "name": "示例物流客户",
  "type": "Customer",
  "contactName": "张三",
  "phone": "13800000000",
  "address": "深圳市南山区"
}
```

启用/停用示例：

```json
{
  "isEnabled": false
}
```

分页查询示例：

```text
GET /api/business-parties?keyword=物流&type=Customer&isEnabled=true&pageNumber=1&pageSize=10
```

## 统一响应示例

成功：

```json
{
  "success": true,
  "message": "操作成功",
  "data": {},
  "errors": null,
  "traceId": null
}
```

失败：

```json
{
  "success": false,
  "message": "请求参数校验失败",
  "data": null,
  "errors": [
    "客户/供应商名称不能为空"
  ],
  "traceId": null
}
```

未知服务器异常不会把堆栈信息返回给调用方，日志中会保留异常和 `traceId`，便于排查。

## 下一步计划

1. 用 EF Core Migration 替换 `EnsureCreated`
2. 发票主表、明细和状态流转
3. 用户登录、JWT 与角色权限
4. 操作日志
5. 发票与客户维度的报表统计
6. Excel 导入导出
7. 单元测试与集成测试

## 学习与面试材料

- [学习疑问记录](docs/学习疑问记录.md)
- [Hermes 交接记录](docs/Hermes交接记录.md)
- [面试讲法](docs/面试讲法.md)

## 项目边界

这是一个用于现代 .NET 能力补强的学习项目，不应描述为已在生产环境落地的系统。可以强调它复用了真实 ERP、发票、对账、结算和报表维护中理解到的业务问题，并用现代 ASP.NET Core 重新实现。
