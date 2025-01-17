using Avalonia.Controls;
using KyoshinEewViewer.Services.Workflows.BuiltinActions;
using ReactiveUI;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KyoshinEewViewer.Services.Workflows;

public record WorkflowActionInfo(Type Type, string DisplayName, Func<WorkflowAction> Create);

[JsonDerivedType(typeof(DummyAction), typeDiscriminator: "Dummy")]
[JsonDerivedType(typeof(MultipleAction), typeDiscriminator: "Multiple")]
[JsonDerivedType(typeof(SendNotificationAction), typeDiscriminator: "SendNotification")]
[JsonDerivedType(typeof(PlaySoundAction), typeDiscriminator: "PlaySound")]
[JsonDerivedType(typeof(WindowActivateAction), typeDiscriminator: "WindowActivate")]
[JsonDerivedType(typeof(WaitAction), typeDiscriminator: "Wait")]
[JsonDerivedType(typeof(LogOutputAction), typeDiscriminator: "LogOutput")]
[JsonDerivedType(typeof(WebhookAction), typeDiscriminator: "Webhook")]
[JsonDerivedType(typeof(ExecuteFileAction), typeDiscriminator: "ExecuteFile")]
public abstract class WorkflowAction : ReactiveObject
{
	static WorkflowAction()
	{
		WorkflowService.RegisterAction<DummyAction>("何もしない");
		WorkflowService.RegisterAction<MultipleAction>("複数アクション実行");
		WorkflowService.RegisterAction<SendNotificationAction>("通知送信");
		WorkflowService.RegisterAction<PlaySoundAction>("音声再生");
		WorkflowService.RegisterAction<WindowActivateAction>("メインウィンドウを最前面に表示");
		WorkflowService.RegisterAction<WaitAction>("指定時間待機");
		WorkflowService.RegisterAction<LogOutputAction>("ログ出力");
		WorkflowService.RegisterAction<WebhookAction>("指定したURLに内容をPOST");
		WorkflowService.RegisterAction<ExecuteFileAction>("指定したファイルを開く(実行)");
	}

	[JsonIgnore]
	public abstract Control DisplayControl { get; }
	public abstract Task ExecuteAsync(WorkflowEvent content);
}

public class DummyAction : WorkflowAction
{
	public override Control DisplayControl => new TextBlock { Text = "何もしないアクションです。\n何も実行されず、中断されることもありません。" };
	public override Task ExecuteAsync(WorkflowEvent content)
		=> Task.CompletedTask;
}
