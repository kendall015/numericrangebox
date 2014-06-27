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

namespace ExampleProject
{
    using Controls;

    public class MainWindowViewModel
    {
        public MainWindowModel Model { get; set; }

        public MainWindowViewModel(MainWindowModel model)
        {
            this.Model = model;

            this.Model.Rb1Options.PropertyChanged += Rb1Options_PropertyChanged;

            this.GenerateSql();
        }

        private void Rb1Options_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.GenerateSql();
        }

        private void GenerateSql()
        {
            string sql =
@"SELECT employee, salary
FROM employees
WHERE ";
            string columnName = "salary";

            if (this.Model.Rb1Options.NumericRangeBoxMode == NumericRangeBoxMode.Value)
            {
                if (string.IsNullOrEmpty(this.Model.Rb1Options.Value))
                {
                    sql += @"1=1   /* No VALUE specified in NumericRangeBox */";
                }
                else
                {
                    switch (this.Model.Rb1Options.NumericRangeBoxComparisonMode)
                    {
                        case NumericRangeBoxComparisonMode.Equals:
                            sql += string.Format("{0} = {1}", columnName, this.Model.Rb1Options.Value);
                            break;

                        case NumericRangeBoxComparisonMode.GreaterThan:
                            sql += string.Format("{0} > {1}", columnName, this.Model.Rb1Options.Value);
                            break;

                        case NumericRangeBoxComparisonMode.GreaterThanOrEqualTo:
                            sql += string.Format("{0} >= {1}", columnName, this.Model.Rb1Options.Value);
                            break;

                        case NumericRangeBoxComparisonMode.LessThan:
                            sql += string.Format("{0} < {1}", columnName, this.Model.Rb1Options.Value);
                            break;

                        case NumericRangeBoxComparisonMode.LessThanOrEqualTo:
                            sql += string.Format("{0} <= {1}", columnName, this.Model.Rb1Options.Value);
                            break;

                        case NumericRangeBoxComparisonMode.NotEquals:
                            sql += string.Format("{0} <> {1}", columnName, this.Model.Rb1Options.Value);
                            break;
                    }
                }
            }
            if (this.Model.Rb1Options.NumericRangeBoxMode == NumericRangeBoxMode.Null)
            {
                sql += string.Format("{0} IS NULL", columnName);
            }
            if (this.Model.Rb1Options.NumericRangeBoxMode == NumericRangeBoxMode.NotNull)
            {
                sql += string.Format("{0} IS NOT NULL", columnName);
            }
            if (this.Model.Rb1Options.NumericRangeBoxMode == NumericRangeBoxMode.RangeInclusive)
            {
                if (string.IsNullOrEmpty(this.Model.Rb1Options.MinValue) || string.IsNullOrEmpty(this.Model.Rb1Options.MaxValue))
                {
                    sql += @"1=1   /* No MAX or MIN specified in NumericRangeBox */";
                }
                else
                {
                    sql += string.Format(@"({0} >= {1} AND {0} <= {2})", columnName, this.Model.Rb1Options.MinValue, this.Model.Rb1Options.MaxValue);
                }
            }
            if (this.Model.Rb1Options.NumericRangeBoxMode == NumericRangeBoxMode.RangeExclusive)
            {
                if (string.IsNullOrEmpty(this.Model.Rb1Options.MinValue) || string.IsNullOrEmpty(this.Model.Rb1Options.MaxValue))
                {
                    sql += @"1=1   /* No MAX or MIN specified in NumericRangeBox */";
                }
                else
                {
                    sql += string.Format(@"({0} > {1} AND {0} < {2})", columnName, this.Model.Rb1Options.MinValue, this.Model.Rb1Options.MaxValue);
                }
            }
            if (this.Model.Rb1Options.NumericRangeBoxMode == NumericRangeBoxMode.All)
            {
                sql += @"1=1";
            }

            this.Model.GeneratedSql = sql;
        }
    }
}