﻿#pragma checksum "..\..\..\ChatRoomList\ChatRoomLists.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "487F9EB8B8B093D643D769B89289D34A4B5CA573A7F4CE73917DB501DBE97991"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using NetDemo;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace NetDemo {
    
    
    /// <summary>
    /// ChatRoomsLists
    /// </summary>
    public partial class ChatRoomsLists : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\ChatRoomList\ChatRoomLists.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel DockPanel;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\ChatRoomList\ChatRoomLists.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock RoomID;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\ChatRoomList\ChatRoomLists.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock RoomName;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\ChatRoomList\ChatRoomLists.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock IDCountText;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\ChatRoomList\ChatRoomLists.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WrapPanel RoomUsers;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\ChatRoomList\ChatRoomLists.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse NewChatNum;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\ChatRoomList\ChatRoomLists.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock newChatCount;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\ChatRoomList\ChatRoomLists.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock CreDate;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/NetDemo;component/chatroomlist/chatroomlists.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ChatRoomList\ChatRoomLists.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.DockPanel = ((System.Windows.Controls.DockPanel)(target));
            
            #line 16 "..\..\..\ChatRoomList\ChatRoomLists.xaml"
            this.DockPanel.MouseEnter += new System.Windows.Input.MouseEventHandler(this.DockPanel_MouseEnter);
            
            #line default
            #line hidden
            
            #line 17 "..\..\..\ChatRoomList\ChatRoomLists.xaml"
            this.DockPanel.MouseLeave += new System.Windows.Input.MouseEventHandler(this.DockPanel_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 2:
            this.RoomID = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.RoomName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.IDCountText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.RoomUsers = ((System.Windows.Controls.WrapPanel)(target));
            return;
            case 6:
            this.NewChatNum = ((System.Windows.Shapes.Ellipse)(target));
            return;
            case 7:
            this.newChatCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.CreDate = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

