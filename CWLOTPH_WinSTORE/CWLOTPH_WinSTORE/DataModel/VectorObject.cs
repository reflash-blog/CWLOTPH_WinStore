using System;
using System.Collections.ObjectModel;

namespace CWLOTPH_WinSTORE.DataModel
{
    public class VectorItem
    {
        public VectorItem(String title, String value, String description)
        {
            this.Title = title;
            this.Description = description;
            this.Value = value;
        }
        public string Title { get; private set; }
        public string Value { get; private set; }
        public string Description { get; private set; }
    }
    public class VectorObject
    {
        public VectorObject(String uniqueid, String title, String subtitle, String description)
        {
            this.UniqueId = uniqueid;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.Items = new ObservableCollection<VectorItem>();
        }
        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public ObservableCollection<VectorItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
