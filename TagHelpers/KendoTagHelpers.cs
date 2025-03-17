using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ProductManagementApp.TagHelpers
{
    [HtmlTargetElement("kendo-grid")]
    public class KendoGridTagHelper : TagHelper
    {
        public string Id { get; set; }
        
        [HtmlAttributeName("grid-data-source")]
        public string DataSource { get; set; }
        
        [HtmlAttributeName("grid-columns")]
        public string Columns { get; set; }
        
        public bool Pageable { get; set; } = true;
        public bool Sortable { get; set; } = true;
        public bool Filterable { get; set; } = true;
        public bool Editable { get; set; } = false;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("id", Id);
            
            var content = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(content.GetContent());
            
            var script = $@"
                <script>
                    $(document).ready(function() {{
                        $('#{Id}').kendoGrid({{
                            dataSource: {DataSource},
                            columns: {Columns},
                            pageable: {Pageable.ToString().ToLower()},
                            sortable: {Sortable.ToString().ToLower()},
                            filterable: {Filterable.ToString().ToLower()},
                            editable: {Editable.ToString().ToLower()}
                        }});
                    }});
                </script>
            ";
            
            output.PostContent.SetHtmlContent(script);
        }
    }
} 