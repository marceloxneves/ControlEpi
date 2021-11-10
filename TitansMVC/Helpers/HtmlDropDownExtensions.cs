using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace TitansMVC.Helpers
{
    public static class HtmlDropDownExtensions
    {
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            Type baseEnumType = Enum.GetUnderlyingType(enumType);
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (FieldInfo field in enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                string text = field.Name;
                string value = Convert.ChangeType(field.GetValue(null), baseEnumType).ToString();
                bool selected = field.GetValue(null).Equals(metadata.Model);

                foreach (var displayAttribute in field.GetCustomAttributes(true).OfType<DisplayAttribute>())
                {
                    text = displayAttribute.GetName();
                }

                items.Add(new SelectListItem()
                {
                    Text = text,
                    Value = value,
                    Selected = selected
                });
            }

            if (metadata.IsNullableValueType)
            {
                items.Insert(0, new SelectListItem { Text = "", Value = "" });
            }

            return SelectExtensions.DropDownListFor(htmlHelper, expression, items, htmlAttributes);
        }

        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }
    }
}