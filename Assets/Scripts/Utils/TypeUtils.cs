using System;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sanicball.Data;

namespace Sanicball.Extra {
	public static class TypeUtils {
		// This doesn't need casting after its called
		public static T As<T>(this string s) {
			var typeDescriptor = TypeDescriptor.GetConverter(typeof(T));
            try {
                return (T)(typeDescriptor.ConvertFromString(s));
            } catch (Exception e) {
                throw new InvalidCastException(string.Format("Unable to cast the {0} to type {1}", s, typeof(T), e));
            }
        }

        public static T As<T>(this UnityEngine.Object obj) {
            var typeDescriptor = TypeDescriptor.GetConverter(typeof(T));
            try {
                return (T)(typeDescriptor.ConvertFrom(obj));
            } catch (Exception e) {
                Type[] types = new Type[] { typeof(int), typeof(long), typeof(float), typeof(double), typeof(bool), typeof(string) };
                if (types.Contains(typeof(T))) {
                    return ((PrimitiveParamValue)obj).paramValue.As<T>();
                } else {
                    throw new InvalidCastException(string.Format("Unable to cast the {0} to type {1}", obj, typeof(T), e));
                }
            }
        }
        public static PrimitiveParamValue ToUnityObject(this int value, PrimitiveParamValue baseObj) {
            //var obj = CreatePrimitiveParamOfType(typeof(int), "");
            var obj = UnityEngine.Object.Instantiate(baseObj);
            obj.paramValue = value.ToString();
            return obj;
        }
        public static PrimitiveParamValue ToUnityObject(this long value, PrimitiveParamValue baseObj) {
            //var obj = CreatePrimitiveParamOfType(typeof(int), "");
            var obj = UnityEngine.Object.Instantiate(baseObj); 
            obj.paramValue = value.ToString();
            return obj;
        }
        public static PrimitiveParamValue ToUnityObject(this float value, PrimitiveParamValue baseObj) {
            //var obj = CreatePrimitiveParamOfType(typeof(int), "");
            var obj = UnityEngine.Object.Instantiate(baseObj);
            obj.paramValue = value.ToString();
            return obj;
        }
        public static PrimitiveParamValue ToUnityObject(this double value, PrimitiveParamValue baseObj) {
            //var obj = CreatePrimitiveParamOfType(typeof(int), "");
            var obj = UnityEngine.Object.Instantiate(baseObj);
            obj.paramValue = value.ToString();
            return obj;
        }
        public static PrimitiveParamValue ToUnityObject(this bool value, PrimitiveParamValue baseObj) {
            //var obj = CreatePrimitiveParamOfType(typeof(int), "");
            var obj = UnityEngine.Object.Instantiate(baseObj);
            obj.paramValue = value.ToString();
            return obj;
        }
        public static PrimitiveParamValue ToUnityObject(this string value, PrimitiveParamValue baseObj) {
            //var obj = CreatePrimitiveParamOfType(typeof(int), "");
            var obj = UnityEngine.Object.Instantiate(baseObj);
            obj.paramValue = value;
            return obj;
        }

        // Original code from: https://stackoverflow.com/questions/299515/reflection-to-identify-extension-methods
        public static MethodInfo GetExtensionMethod(this Type t, string name) {
            var query = from type in Assembly.GetExecutingAssembly().GetTypes()
                        where !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static
                            | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false)
                        where method.GetParameters()[0].ParameterType == t
                        where method.Name == name
                        select method;

            return query.SingleOrDefault();
        }

        public static PrimitiveParamValue CreatePrimitiveParamOfType(Type t, string paramName) {
            var value = ScriptableObject.CreateInstance<PrimitiveParamValue>();
            value.paramValue = Activator.CreateInstance(t).ToString();
            value.paramName = paramName;
            value.name = paramName;
            return value;
        }
    }
}