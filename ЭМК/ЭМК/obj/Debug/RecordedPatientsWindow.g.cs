// Updated by XamlIntelliSenseFileGenerator 26.05.2025 8:48:15
#pragma checksum "..\..\RecordedPatientsWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F53A118B1190C1D5D4ED53CA3BA8871F59E1DB85718CA3813CA59A4A8424ABA3"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

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
using ЭМК;


namespace ЭМК
{


	/// <summary>
	/// RecordedPatientsWindow
	/// </summary>
	public partial class RecordedPatientsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
	{


#line 21 "..\..\RecordedPatientsWindow.xaml"
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		internal System.Windows.Controls.TextBlock txtDoctorInfo;

#line default
#line hidden


#line 49 "..\..\RecordedPatientsWindow.xaml"
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		internal System.Windows.Controls.Button Next;

#line default
#line hidden


#line 80 "..\..\RecordedPatientsWindow.xaml"
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		internal System.Windows.Controls.Button ExitBt;

#line default
#line hidden

		private bool _contentLoaded;

		/// <summary>
		/// InitializeComponent
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (_contentLoaded)
			{
				return;
			}
			_contentLoaded = true;
			System.Uri resourceLocater = new System.Uri("/ЭМК;component/recordedpatientswindow.xaml", System.UriKind.Relative);

#line 1 "..\..\RecordedPatientsWindow.xaml"
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
		void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
				case 1:
					this.txtDoctorInfo = ((System.Windows.Controls.TextBlock)(target));
					return;
				case 2:
					this.Next = ((System.Windows.Controls.Button)(target));

#line 49 "..\..\RecordedPatientsWindow.xaml"
					this.Next.Click += new System.Windows.RoutedEventHandler(this.Next_Click);

#line default
#line hidden
					return;
				case 3:
					this.ExitBt = ((System.Windows.Controls.Button)(target));

#line 80 "..\..\RecordedPatientsWindow.xaml"
					this.ExitBt.Click += new System.Windows.RoutedEventHandler(this.ExitBt_Click);

#line default
#line hidden
					return;
			}
			this._contentLoaded = true;
		}

		internal System.Windows.Controls.DataGrid AppointmentsDataGrid;
	}
}

