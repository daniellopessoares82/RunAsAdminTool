#nullable enable

namespace RunAsAdminTool;

/// <summary>
/// Designer file for the MainForm class.
/// Contains the Windows Forms designer generated code.
/// </summary>
partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        
        // NotifyIcon setup
        notifyIcon = new NotifyIcon(components);
        notifyIcon.Text = "Run As Admin Tool";
        notifyIcon.Visible = true;
        notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
        
        // ContextMenuStrip setup
        contextMenuStrip = new ContextMenuStrip(components);
        contextMenuStrip.Name = "contextMenuStrip";
        contextMenuStrip.Size = new Size(150, 30);
        
        // Associate context menu with notify icon
        notifyIcon.ContextMenuStrip = contextMenuStrip;
        
        // Form setup
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(300, 200);
        Name = "MainForm";
        ShowInTaskbar = false;
        Text = "Run As Admin Tool";
        WindowState = FormWindowState.Minimized;
        
        // Event handlers
        Resize += MainForm_Resize;
    }

    private NotifyIcon notifyIcon = null!;
    private ContextMenuStrip contextMenuStrip = null!;
}
