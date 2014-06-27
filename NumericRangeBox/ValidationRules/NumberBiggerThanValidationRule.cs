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
    using System.Globalization;
    using System.Windows.Controls;

    public class NumberBiggerThanValidationRule : ValidationRule
    {
        public string OtherValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            decimal otherValueDecimal;
            if (!decimal.TryParse(this.OtherValue, out otherValueDecimal))
            {
                // this.OtherValue is not a decimal, so don't compare
                return new ValidationResult(true, null);
            }

            if (value == null)
            {
                // value is null, so don't compare
                return new ValidationResult(true, null);
            }

            decimal thisValueDecimal;
            if (!decimal.TryParse(value.ToString(), out thisValueDecimal))
            {
                // value is not a decimal, so don't compare
                return new ValidationResult(true, null);
            }

            if (thisValueDecimal > otherValueDecimal)
            {
                return new ValidationResult(true, null);
            }
            string message = string.Format("'{0}' Is Less Than '{1}'", thisValueDecimal, this.OtherValue);
            return new ValidationResult(false, message);
        }
    }
}