using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mozlite.Mvc.TagHelpers.Bootstrap
{
    /// <summary>
    /// ��ѡ���б��ǩ��
    /// </summary>
    public abstract class RadioboxListTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// ���ơ�
        /// </summary>
        [HtmlAttributeName("name")]
        public string Name { get; set; }

        /// <summary>
        /// ��ǰֵ��
        /// </summary>
        [HtmlAttributeName("value")]
        public string Value { get; set; }

        /// <summary>
        /// ÿ����ʽ���͡�
        /// </summary>
        [HtmlAttributeName("iclass")]
        public string ItemClass { get; set; }

        /// <summary>
        /// ÿ��ѡ����ʽ���͡�
        /// </summary>
        [HtmlAttributeName("istyle")]
        public CheckedStyle CheckedStyle { get; set; }

        /// <summary>
        /// ���ʲ����ֵ�ǰ��ǩʵ����
        /// </summary>
        /// <param name="context">��ǰHTML��ǩ�����ģ�������ǰHTML�����Ϣ��</param>
        /// <param name="output">��ǰ��ǩ���ʵ�������ڳ��ֱ�ǩ�����Ϣ��</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var items = new Dictionary<string, string>();
            Init(items);
            foreach (var item in items)
            {
                output.Content.AppendHtml(Create(item.Key, item.Value, string.Equals(item.Value, (string)Value)));
            }
            var builder = new TagBuilder("div");
            builder.AddCssClass("moz-radioboxlist");
            output.TagName = builder.TagName;
            output.MergeAttributes(builder);
        }

        /// <summary>
        /// ���Ӹ�ѡ��Ŀ�б��ı�/ֵ��
        /// </summary>
        /// <param name="items">��ѡ����Ŀ�б�ʵ����</param>
        protected abstract void Init(IDictionary<string, string> items);

        private TagBuilder Create(string text, string value, bool isChecked)
        {
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("moz-radiobox");
            if (ItemClass != null)
                wrapper.AddCssClass(ItemClass);
            if (isChecked)
                wrapper.AddCssClass("checked");
            wrapper.AddCssClass("checked-style-" + CheckedStyle.ToString().ToLower());

            var input = new TagBuilder("input");
            input.MergeAttribute("type", "radio");
            input.MergeAttribute("name", Name);
            input.MergeAttribute("value", value);
            if (isChecked)
                input.MergeAttribute("checked", "checked");
            wrapper.InnerHtml.AppendHtml(input);

            var label = new TagBuilder("label");
            label.AddCssClass("box-wrapper");
            label.InnerHtml.AppendHtml("<div class=\"box-checked\"></div>");
            wrapper.InnerHtml.AppendHtml(label);

            var span = new TagBuilder("span");
            span.InnerHtml.AppendHtml(text);
            wrapper.InnerHtml.AppendHtml(span);
            return wrapper;
        }
    }
}