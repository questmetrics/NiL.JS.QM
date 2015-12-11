﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace Examples._4_Namespaces_and_external_types
{
    public sealed class Using_of_attributes : ExamplesFramework.Example
    {
        private sealed class TestClass
        {
            public string RegularProperty { get; } = "Regular property";

            [DoNotEnumerate]
            public string NonEnumerableProperty { get; } = "Non-enumerable property";

            [NotConfigurable]
            public string NonConfigurableProperty { get; } = "Non-configerable property";

            [ReadOnly]
            public string ReadOnlyField = "Read only field";

            public string RegularField = "Regular field";

            [Hidden]
            public string HiddenProperty { get; set; } = "Hidden property";

            [DoNotDelete]
            public string NonDeletableProperty { get; } = "Non-deletable property";

            public IEnumerable<int> PropertyWithCompositeType {[return: MyValueConverter] get; } = new[] { 1, 2, 3, 4, 5 };

            public void MethodWithConverter([MyValueConverter] string[] parts)
            {
                Console.WriteLine(parts.Aggregate((x, result) => result + x));
            }
        }

        private sealed class MyValueConverter : ConvertValueAttribute
        {
            public override object From(object source)
            {
                var enumerable = source as IEnumerable<int>;
                if (enumerable != null)
                {
                    return enumerable.Aggregate((x, sum) => sum + x);
                }

                return null;
            }

            public override object To(object source)
            {
                var @string = source as string;
                if (@string != null)
                {
                    return @string.Select(x => x.ToString()).ToArray();
                }

                return null;
            }
        }

        public override void Run()
        {
            var context = new Context();
            var instance = new TestClass();
            context.DefineVariable("instance").Assign(JSValue.Wrap(instance));

            example1(context);

            Console.WriteLine();

            example2(context);

            Console.WriteLine();

            example3(context);

            Console.WriteLine();

            example4(context, instance);

            Console.WriteLine();

            example5(context, instance);

            Console.WriteLine();

            example6(context, instance);

            Console.WriteLine();

            example7(context);
        }

        private static void example1(Context context)
        {
            context.Eval(@"
let result = false;
for (let property in instance)
{
    if (property === 'NonEnumerableProperty')
        result = true;
}

console.log(result); // Console: false
");
        }

        private static void example2(Context context)
        {
            context.Eval(@"
Object.defineProperty(instance, 'NonConfigurableProperty', { configerable: true });
let result = Object.getOwnPropertyDescriptor(instance, 'NonConfigurableProperty').configurable;

console.log(result); // Console: false
");
        }

        private static void example3(Context context)
        {
            context.Eval(@"
instance.ReadOnlyField = 'my value';
let result = instance.ReadOnlyField === 'my value';

console.log(result); // Console: false
");
        }

        private static void example4(Context context, TestClass instance)
        {
            context.Eval(@"
instance.RegularField = 'my value';
let result = instance.RegularField === 'my value';

console.log(result); // Console: true
");
            Console.WriteLine(instance.RegularField); // Console: my value
        }

        private static void example5(Context context, TestClass instance)
        {
            context.Eval(@"
let result = instance.HiddenProperty === undefined;

console.log(result); // Console: true
");
            context.Eval(@"
instance.HiddenProperty = 'my value';
let result = instance.HiddenProperty === 'my value';

console.log(result); // Console: true
");
            Console.WriteLine(instance.HiddenProperty); // Console: Hidden property
        }

        private static void example6(Context context, TestClass instance)
        {
            context.Eval(@"
let result = instance.PropertyWithCompositeType === 15;

console.log(result); // Console: true
");
            Console.WriteLine(instance.PropertyWithCompositeType); // Console: System.Int32[]
        }

        private static void example7(Context context)
        {
            context.Eval(@"
instance.MethodWithConverter('54321'); // Console: 12345
");
        }
    }
}
