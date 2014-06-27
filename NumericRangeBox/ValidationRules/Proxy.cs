/*
 * This is free and unencumbered software released into the public domain.
 *
 * Anyone is free to copy, modify, publish, use, compile, sell, or
 * distribute this software, either in source code form or as a compiled
 * binary, for any purpose, commercial or non-commercial, and by any
 * means.
 *
 * In jurisdictions that recognize copyright laws, the author or authors
 * of this software dedicate any and all copyright interest in the
 * software to the public domain. We make this dedication for the benefit
 * of the public at large and to the detriment of our heirs and
 * successors. We intend this dedication to be an overt act of
 * relinquishment in perpetuity of all present and future rights to this
 * software under copyright law.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * For more information, please refer to <http://unlicense.org/>
 */

namespace Controls.ValidationRules
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Threading;

    /// <summary>
    /// The concept is a "Proxy" FrameworkElement with two DependencyProperties,
    /// one Input and one Output. The Input property is tied to the Output such
    /// that when Input changes it is applied to Output.
    /// </summary>
    /// <remarks>
    /// Code adapted from: http://www.11011.net/wpf-binding-properties
    /// </remarks>
    public class Proxy : FrameworkElement
    {
        public static readonly DependencyProperty InProperty;
        public static readonly DependencyProperty OutProperty;

        public Proxy()
        {
            Visibility = Visibility.Collapsed;
        }

        static Proxy()
        {
            FrameworkPropertyMetadata inMetadata = new FrameworkPropertyMetadata(
                delegate(DependencyObject p, DependencyPropertyChangedEventArgs args)
                {
                    if (BindingOperations.GetBinding(p, OutProperty)==null)
                    {
                        return;
                    }
                    var proxy = p as Proxy;
                    if (proxy != null)
                    {
                        proxy.Out = args.NewValue;
                    }
                }) { BindsTwoWayByDefault = false, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };

            InProperty = DependencyProperty.Register("In",
                typeof(object),
                typeof(Proxy),
                inMetadata);

            FrameworkPropertyMetadata outMetadata = new FrameworkPropertyMetadata(
                delegate(DependencyObject p, DependencyPropertyChangedEventArgs args)
                {
                    ValueSource source = DependencyPropertyHelper.GetValueSource(p, args.Property);

                    if (source.BaseValueSource != BaseValueSource.Local)
                    {
                        Proxy proxy = p as Proxy;
                        if (proxy != null)
                        {
                            object expected = proxy.In;
                            if (!ReferenceEquals(args.NewValue, expected))
                            {
                                Dispatcher.CurrentDispatcher.BeginInvoke(
                                    DispatcherPriority.DataBind, new Action(delegate
                                        {
                                            proxy.Out = proxy.In;
                                        }));
                            }
                        }
                    }
                }) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };

            OutProperty = DependencyProperty.Register("Out",
                typeof(object),
                typeof(Proxy),
                outMetadata);
        }

        public object In
        {
            get { return this.GetValue(InProperty); }
            set { this.SetValue(InProperty, value); }
        }

        public object Out
        {
            get { return this.GetValue(OutProperty); }
            set { this.SetValue(OutProperty, value); }
        }
    }
}