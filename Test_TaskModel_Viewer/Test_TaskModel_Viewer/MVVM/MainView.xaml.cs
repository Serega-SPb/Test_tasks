using System.Windows.Controls;
using System.Windows.Media;
using Test_TaskModel_Viewer.Controls;

namespace Test_TaskModel_Viewer.MVVM
{
    public partial class MainView
    {

	    private ScrollViewer _calendar;
	    private ScrollViewer _table;

		public MainView()
        {
            InitializeComponent();
	        Focus();
        }

	    private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
	    {
			if (_calendar == null)
				_calendar = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(Calendar.list,0),0) as ScrollViewer;

		    if (_table == null)
			    _table = Table.Scroller;

		    if (sender is GraphCalendar)
		    {
			    _table.ScrollToVerticalOffset(e.VerticalOffset);
			    _table.ScrollToHorizontalOffset(e.HorizontalOffset);
		    }
		    else if (sender is TreeTable)
		    {
			    _calendar.ScrollToVerticalOffset(e.VerticalOffset);
			    _calendar.ScrollToHorizontalOffset(e.HorizontalOffset);
		    }
			
		}
    }
}
