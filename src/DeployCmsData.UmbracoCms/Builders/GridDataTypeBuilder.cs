﻿using DeployCmsData.UmbracoCms.Models;
using DeployCmsData.UmbracoCms.Services;
using System;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Validation;
using ProperyEditors = Umbraco.Core.Constants.PropertyEditors;


namespace DeployCmsData.UmbracoCms.Builders
{
    public class GridDataTypeBuilder
    {
        public const string PreValueItemsName = "items";
        public const string PreValueRteName = "rte";

        public GridItemsPreValue GridItemsPreValue { get; set; }
        public GridRtePreValue GridRtePreValue { get; set; }

        private IDataTypeService DataTypeService { get; set; }
        private string NameValue { get; set; }
        private Guid KeyValue { get; set; }

        public GridDataTypeBuilder(Guid key)
        {
            Setup(UmbracoContext.Current.Application.Services.DataTypeService);
            KeyValue = key;
        }

        public GridDataTypeBuilder(IDataTypeService dataTypeService, Guid key)
        {
            Setup(dataTypeService);
            KeyValue = key;
        }

        public void Setup(IDataTypeService dataTypeService)
        {
            Requires.NotNull(dataTypeService, nameof(dataTypeService));

            DataTypeService = dataTypeService;
            GridItemsPreValue = new GridItemsPreValue();
            GridRtePreValue = new GridRtePreValue();
            GridItemsPreValue.Columns = 12;
            KeyValue = Guid.Empty;

            GridItemsPreValue.Styles.Add(new Style()
            {
                Label = "Set a background image",
                Description = "Set a row background",
                Key = "background-image",
                View = "imagepicker",
                Modifier = "url({0})"
            });

            GridItemsPreValue.Config.Add(new Config()
            {
                Label = "Class",
                Description = "Set a css class",
                Key = "class",
                View = "textstring"
            });

            GridRtePreValue.Dimensions = new Dimensions(500);
            GridRtePreValue.MaxImageSize = 500;
        }

        public GridDataTypeBuilder Name(string gridName)
        {
            Requires.NotNullOrWhiteSpace(gridName, nameof(gridName));

            NameValue = gridName;
            return this;
        }

        public GridDataTypeBuilder AddLayout(string layoutName, params int[] gridColumns)
        {
            Requires.NotNullOrWhiteSpace(layoutName, nameof(layoutName));
            Requires.NotNull(gridColumns, nameof(gridColumns));

            var template = new Models.Template
            {
                Name = layoutName
            };

            foreach (var column in gridColumns)
            {
                template.Sections.Add(new Models.Section(column));
            }

            GridItemsPreValue.Layouts.Add(template);

            return this;
        }

        public GridDataTypeBuilder AddRow(string rowName, params int[] areas)
        {
            Requires.NotNull(rowName, nameof(rowName));
            Requires.NotNull(areas, nameof(areas));

            var layout = new GridLayout
            {
                Label = rowName,
                Name = rowName
            };

            foreach (var area in areas)
            {
                layout.Areas.Add(new Area(area));
            }

            return this;
        }

        public GridDataTypeBuilder AddStandardLayouts()
        {
            AddLayout("1 column layout", 12);
            AddLayout("2 column layout", 4, 8);

            return this;
        }

        public GridDataTypeBuilder AddStandardRows()
        {
            AddRow("Headline", 12);
            AddRow("Article", 4, 8);

            return this;
        }

        public GridDataTypeBuilder AddStandardToolBar()
        {
            GridRtePreValue.ToolBar.Add("code");
            GridRtePreValue.ToolBar.Add("styleselect");
            GridRtePreValue.ToolBar.Add("bold");
            GridRtePreValue.ToolBar.Add("italic");
            GridRtePreValue.ToolBar.Add("alignleft");
            GridRtePreValue.ToolBar.Add("aligncenter");
            GridRtePreValue.ToolBar.Add("alignright");
            GridRtePreValue.ToolBar.Add("bullist");
            GridRtePreValue.ToolBar.Add("numlist");
            GridRtePreValue.ToolBar.Add("outdent");
            GridRtePreValue.ToolBar.Add("indent");
            GridRtePreValue.ToolBar.Add("link");
            GridRtePreValue.ToolBar.Add("umbmediapicker");
            GridRtePreValue.ToolBar.Add("umbmacro");
            GridRtePreValue.ToolBar.Add("umbembeddialog");
            return this;
        }

        public GridDataTypeBuilder AddToolBarOption(string option)
        {
            GridRtePreValue.ToolBar.Add(option);
            return this;
        }

        public GridDataTypeBuilder DeleteGrid(string gridName)
        {
            Requires.NotNullOrWhiteSpace(gridName, nameof(gridName));

            DataTypeBuilder.DeleteDataTypeByName(gridName, DataTypeService);
            return this;
        }

        public void DeleteGrid(Guid gridId)
        {
            DataTypeBuilder.DeleteDataTypeById(gridId, DataTypeService);
        }

        public IDataTypeDefinition Build()
        {
            var newGridDataType = new DataTypeDefinition(-1, ProperyEditors.GridAlias)
            {
                Name = NameValue
            };

            if (KeyValue != Guid.Empty)
            {
                newGridDataType.Key = KeyValue;
            }

            var preValues = new System.Collections.Generic.Dictionary<string, PreValue>
            {
                { PreValueItemsName, new PreValue(JsonHelper.SerializePreValueObject(GridItemsPreValue)) },
                { PreValueRteName, new PreValue(JsonHelper.SerializePreValueObject(GridRtePreValue)) }
            };

            DataTypeService.SaveDataTypeAndPreValues(newGridDataType, preValues);

            return newGridDataType;
        }
    }
}