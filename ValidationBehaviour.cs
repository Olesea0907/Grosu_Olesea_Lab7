using Microsoft.Maui.Controls;
using System;

namespace Grosu_Olesea_Lab7
{
    public class ValidationBehaviour : Behavior<Editor>
    {
        protected override void OnAttachedTo(Editor editor)
        {
            editor.TextChanged += OnEditorTextChanged; 
            base.OnAttachedTo(editor);
        }

        protected override void OnDetachingFrom(Editor editor)
        {
            editor.TextChanged -= OnEditorTextChanged; 
            base.OnDetachingFrom(editor);
        }

        private void OnEditorTextChanged(object sender, TextChangedEventArgs args)
        {
            ((Editor)sender).BackgroundColor = string.IsNullOrEmpty(args.NewTextValue)
                ? Color.FromArgb("#AA4A44") 
                : Color.FromArgb("#FFFFFF"); 
        }
    }
}
