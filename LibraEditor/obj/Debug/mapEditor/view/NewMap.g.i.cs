﻿#pragma checksum "..\..\..\..\mapEditor\view\NewMap.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "00AD92E99F5F428B34A441D0E300E495"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using LibraEditor.mapEditor.view.newMap;
using MahApps.Metro.Controls;
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


namespace LibraEditor.mapEditor.view.newMap {
    
    
    /// <summary>
    /// NewMap
    /// </summary>
    public partial class NewMap : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\mapEditor\view\NewMap.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox projectTypeComboBox;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\mapEditor\view\NewMap.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox mapNameTextBox;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\mapEditor\view\NewMap.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton obliqueRadioButton;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\mapEditor\view\NewMap.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.NumericUpDown tileWidthNumeric;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\mapEditor\view\NewMap.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label tileHeightLabel;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\mapEditor\view\NewMap.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.NumericUpDown tileHeightNumeric;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\mapEditor\view\NewMap.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.NumericUpDown tileRowsNumeric;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\mapEditor\view\NewMap.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.NumericUpDown tileColsNumeric;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\mapEditor\view\NewMap.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock mapFloderTextBlock;
        
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
            System.Uri resourceLocater = new System.Uri("/LibraEditor;component/mapeditor/view/newmap.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\mapEditor\view\NewMap.xaml"
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
            this.projectTypeComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.mapNameTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            
            #line 32 "..\..\..\..\mapEditor\view\NewMap.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.OnMapTypeChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.obliqueRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 33 "..\..\..\..\mapEditor\view\NewMap.xaml"
            this.obliqueRadioButton.Checked += new System.Windows.RoutedEventHandler(this.OnMapTypeChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.tileWidthNumeric = ((MahApps.Metro.Controls.NumericUpDown)(target));
            return;
            case 6:
            this.tileHeightLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.tileHeightNumeric = ((MahApps.Metro.Controls.NumericUpDown)(target));
            return;
            case 8:
            this.tileRowsNumeric = ((MahApps.Metro.Controls.NumericUpDown)(target));
            return;
            case 9:
            this.tileColsNumeric = ((MahApps.Metro.Controls.NumericUpDown)(target));
            return;
            case 10:
            this.mapFloderTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            
            #line 50 "..\..\..\..\mapEditor\view\NewMap.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OnOpenDir);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 52 "..\..\..\..\mapEditor\view\NewMap.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OnCreateMap);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

