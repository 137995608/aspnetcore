using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mozlite.Mvc.TagHelpers.Bootstrap
{
    /// <summary>
    /// ��ѡ���б��ǩ��
    /// </summary>
    public abstract class CheckboxListTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// ���ơ�
        /// </summary>
        [HtmlAttributeName("name")]
        public string Name { get; set; }

        /// <summary>
        /// �ԡ�,���ָ�ֵ��
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
        /// ��������ģ�͡�
        /// </summary>
        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        /// <summary>
        /// ��ʼ����ǰ��ǩ�����ġ�
        /// </summary>
        /// <param name="context">��ǰHTML��ǩ�����ģ�������ǰHTML�����Ϣ��</param>
        public override void Init(TagHelperContext context)
        {
            if (string.IsNullOrEmpty(Name) && For != null)
            {
                Name = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
                if (Value == null)
                {
                    if (For.Model is string str)
                        Value = str;
                    else if (For.Model is IEnumerable array)
                        Value = array.Join();
                    else
                        Value = For.Model?.ToString();
                }
            }
        }

        /// <summary>
        /// ���ʲ����ֵ�ǰ��ǩʵ����
        /// </summary>
        /// <param name="context">��ǰHTML��ǩ�����ģ�������ǰHTML�����Ϣ��</param>
        /// <param name="output">��ǰ��ǩ���ʵ�������ڳ��ֱ�ǩ�����Ϣ��</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Value != null)
                Value = $",{Value},";
            var items = new Dictionary<string, string>();
            Init(items);
            foreach (var item in items)
            {
                output.Content.AppendHtml(Create(item.Key, item.Value, IsChecked(item.Value)));
            }
            var builder = new TagBuilder("div");
            builder.AddCssClass("moz-checkboxlist");
            output.SetTag(builder);
        }

        /// <summary>
        /// �ж�ѡ�е�״̬��
        /// </summary>
        /// <param name="current">��ǰ��Ŀֵ��</param>
        /// <returns>�����жϽ����</returns>
        protected virtual bool IsChecked(string current)
        {
            return Value?.IndexOf($",{current},") >= 0;
        }

        /// <summary>
        /// ���Ӹ�ѡ��Ŀ�б��ı�/ֵ��
        /// </summary>
        /// <param name="items">��ѡ����Ŀ�б�ʵ����</param>
        protected abstract void Init(IDictionary<string, string> items);

        private TagBuilder Create(string text, string value, bool isChecked)
        {
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("moz-checkbox");
            if (ItemClass != null)
                wrapper.AddCssClass(ItemClass);
            if (isChecked)
                wrapper.AddCssClass("checked");
            wrapper.AddCssClass("checked-style-" + CheckedStyle.ToString().ToLower());

            var input = new TagBuilder("input");
            input.MergeAttribute("type", "checkbox");
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