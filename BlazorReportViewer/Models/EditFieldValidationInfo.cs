using DevExpress.XtraPrinting;
using Microsoft.AspNetCore.Components;

namespace BlazorReportViewer.Models {
    public struct EditFieldValidationInfo {
        public int PageIndex { get; set; }
        public string EditFieldID { get; set; }
        public VisualBrick Brick { get; set; }
        public object Tag { get; set; }
        public EventCallback HighlightBrick { get; set; }
    }
}
