using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Gravical.Poker.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class TestBase
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        #region ActExpectingException

        protected void ActExpectingArgumentNullException(string name, Action action)
        {
            ActExpectingException(typeof(ArgumentNullException), $"Value cannot be null. (Parameter '{name}')", action);
        }

        protected void ActExpectingArgumentException(string name, Action action)
        {
            ActExpectingArgumentException(name, "Invalid argument", action);
        }

        protected void ActExpectingArgumentException(string name, string message, Action action)
        {
            ActExpectingException(typeof(ArgumentException), $"{message} (Parameter '{name}')", action);
        }

        protected void ActExpectingInvalidOperationException(string message, Action action)
        {
            ActExpectingException(typeof(InvalidOperationException), message, action);
        }

        protected void ActExpectingException(Type type, string message, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(type, ex.GetType(), "Exception type was not expected");
                Assert.AreEqual(message, ex.Message, "Exception message was not expected");
                Console.WriteLine(ex.Message);
                return;
            }

            Assert.Fail("Expected an exception but none occured");
        }

        #endregion

        #region Reflection

        protected bool PublicConstructorExists(Type type)
        {
            return type.GetConstructors().Any(_ => _.IsPublic);
        }

        protected bool PropertySetAvailable(Type type, string name)
        {
            var info = type.GetProperty(name);
            if (info == null) throw new Exception($"Property {name} not found");

            var set = info.GetSetMethod();
            return set?.IsPublic ?? false;
        }

        protected void SetAnyProperty(object instance, string name, object value)
        {
            var type = instance.GetType();
            if (type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException(nameof(name), $"Property {name} was not found in Type {type.FullName}");
            type.InvokeMember(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, instance, new object[] { value });

        }

        protected T BinaryToStruct<T>(byte[] source) where T : struct
        {
            var handle = GCHandle.Alloc(source, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        #endregion
    }
}