using System.Windows;
using System.Windows.Controls;
using Test_TaskModel_Viewer.Classes;

namespace Test_TaskModel_Viewer.Controls
{

    public partial class TreeTable
    {
        public TreeTable()
        {
            InitializeComponent();
        }

		public static readonly DependencyProperty ModelRelationProperty = DependencyProperty.Register(
		    "ModelRelation", typeof(ModelRelation), typeof(TreeTable), new PropertyMetadata(default(ModelRelation)));

	    public ModelRelation ModelRelation
	    {
		    get { return (ModelRelation) GetValue(ModelRelationProperty); }
		    set { SetValue(ModelRelationProperty, value); }
	    }

	    private void ExpandedChanged(object sender, RoutedEventArgs e)
	    {
		    if (sender is Expander exp && exp.DataContext is ModelRelation mr)
			    mr.IsExpanded = exp.IsExpanded;
	    }
    }
}
