﻿#pragma checksum "..\..\..\User\UserPic.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6CEDEAA905C76869E0BE09179CDC9F04B6B4BEDCEDFE0865753A6ABEF21A26EF"
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
    /// UserPic
    /// </summary>
    public partial class UserPic : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\User\UserPic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid picGrid;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\User\UserPic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BackBtn;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\User\UserPic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgView;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\User\UserPic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button setBtn;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\User\UserPic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgBitmapView;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\User\UserPic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button mypic;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\User\UserPic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button mypic_result;
        
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
            System.Uri resourceLocater = new System.Uri("/NetDemo;component/user/userpic.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\User\UserPic.xaml"
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
            this.picGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.BackBtn = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\User\UserPic.xaml"
            this.BackBtn.Click += new System.Windows.RoutedEventHandler(this.BackBtn_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.imgView = ((System.Windows.Controls.Image)(target));
            return;
            case 4:
            this.setBtn = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\User\UserPic.xaml"
            this.setBtn.Click += new System.Windows.RoutedEventHandler(this.setBtn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.imgBitmapView = ((System.Windows.Controls.Image)(target));
            return;
            case 6:
            this.mypic = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\User\UserPic.xaml"
            this.mypic.Click += new System.Windows.RoutedEventHandler(this.mypic_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.mypic_result = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\User\UserPic.xaml"
            this.mypic_result.Click += new System.Windows.RoutedEventHandler(this.mypic_result_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

