﻿using Microsoft.AspNetCore.Components;

namespace SwashbucklerDiary.Components
{
    public partial class SwiperTabItem
    {
        [Parameter]
        public ElementReference Ref { get; set; }
        [Parameter]
        public string? Id { get; set; }
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
        [Parameter]
        public RenderFragment? FixContent { get; set; }
    }
}
