using System.Collections.Generic;

namespace ComponentGenerator.ApplicationBuilder.Model
{
    internal class ApplicationModel
    {
        public string ComponentSection { get; internal set; }
        public List<string> ReferencedComponents { get; internal set; }
    }
}