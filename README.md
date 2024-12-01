# SimpleBilling API
This project simulates a simple billing API, inspired by <a href="https://github.com/HusseinYoussef/Billing-API">HusseinYoussef/Billing-API</a>.<br/>
Implemented using CQRS, Repository Pattern and the By Technical Architecture. <br/>

<h3>Used Technologies</h3>
<div style="display: flex; gap: 10px;">
    <img height="32" width="32" src="https://cdn.simpleicons.org/dotnet" alt="dotnet" />&nbsp;
    <img height="32" width="32" src="https://cdn.simpleicons.org/swagger" alt="swagger" />&nbsp;
    <img height="32" width="32" src="https://cdn.simpleicons.org/zedindustries" alt="zedindustries" />&nbsp;
    <img height="32" width="32" src="https://cdn.simpleicons.org/postgresql" alt="postgresql" />&nbsp;
</div>

### Dependencies (NuGet Packages)
<ul>
    <li>Base Project
        <ul>
            <li>FluentValidation</li>
            <li>Dapper</li>
            <li>Npgsql</li>
            <li>Serilog</li>
            <li>Serilog.Sinks.Console</li>
            <li><a href="https://github.com/RicoSuter/NSwag">NSwag</a></li>
            <li><a href="https://github.com/JasperFx/wolverine">Wolverine</a></li>
        </ul>
    </li></br>
    <li>Unit Tests
        <ul>
            <li>coverlet.collector</li>
            <li>FluentAssertions</li>
            <li>Microsoft.NET.Test.Sdk</li>
            <li>Moq</li>
            <li>xunit</li>
            <li>xunit.runner.visualstudio</li>
        </ul>
    </li>
</ul>

<table style="width: 100%; text-align: center; border-spacing: 20px;">
  <tr>
    <td style="border: 1px solid #ddd; padding: 10px;">
      <img src="https://github.com/user-attachments/assets/1f627501-113c-4b8f-b565-bec4af9ce51b" alt="Zed Editor" width="870">
    </td>
  </tr>
  <tr>
    <td style="border: 1px solid #ddd; padding: 10px;">
      <img src="https://github.com/user-attachments/assets/6b96d3bc-addb-423f-a399-47b065556ea4" alt="Swagger UI" width="870">
    </td>
  </tr>
</table>

<h3>References 📚</h3>
<a href="https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-8.0&tabs=visual-studio">Get started with NSwag and ASP.NET Core</a><br/>
<a href="https://wolverinefx.net/guide/durability/marten/event-sourcing.html">Aggregate Handlers and Event Sourcing</a><br/>
