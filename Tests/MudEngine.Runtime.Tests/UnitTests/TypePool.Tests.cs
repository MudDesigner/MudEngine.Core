using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine.Tests.Fixture;

namespace MudDesigner.MudEngine.Tests
{
    [TestClass]
    public class TypePoolTests
    {
        [TestInitialize]
        public void Setup()
        {
            TypePool.ClearPool();
        }

        #region GetType tests
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Add_type_adds_a_type_to_cache()
        {
            // Arrange
            Type fixtureType = typeof(ComponentFixture);

            // Act
            TypePool.AddType(fixtureType);
            Type cachedType = TypePool.GetType<ComponentFixture>();

            // Assert
            Assert.IsTrue(object.ReferenceEquals(fixtureType, cachedType));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [ExpectedException(typeof(ArgumentNullException))]
        [Owner("Johnathon Sullinger")]
        public void Clear_type_from_pool_with_null_instance_throws_exception()
        {
            // Act
            TypePool.ClearTypeFromPool(null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Clear_type_from_pool_clears_cache()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            TypePool.AddType(typeof(TypePoolFixture));

            // Act
            TypePool.ClearTypeFromPool(fixture);

            // Assert
            Assert.IsFalse(TypePool.HasTypeInCache<TypePoolFixture>());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Has_type_in_cache_returns_true_when_type_is_cached()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            TypePool.AddType(typeof(TypePoolFixture));

            // Act
            bool exists = TypePool.HasTypeInCache(fixture);

            // Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Has_type_in_cache_returns_false_when_type_is_never_cached()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            
            // Act
            bool exists = TypePool.HasTypeInCache(fixture);

            // Assert
            Assert.IsFalse(exists);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Add_type_twice_does_not_return_duplicates()
        {
            // Arrange
            Type fixtureType = typeof(ComponentFixture);
            Type fixtureType2 = typeof(ComponentFixture);

            // Act
            TypePool.AddType(fixtureType);
            TypePool.AddType(fixtureType2);

            IEnumerable<Type> types = TypePool.GetTypes(type => type == typeof(ComponentFixture));

            // Assert
            Assert.AreEqual(1, types.Count());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_by_name_returns_type()
        {
            // Arrange
            Type componentFixture = typeof(ComponentFixture);
            Type typePoolFixture = typeof(TypePoolFixture);
            TypePool.AddType(componentFixture);
            TypePool.AddType(typePoolFixture);

            // Act
            Type returnedType = TypePool.GetTypeByName(typeof(TypePoolFixture).Name);

            // Assert
            Assert.IsNotNull(returnedType);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_from_pool_from_generic()
        {
            // Act
            Type fixtureType = TypePool.GetType<ComponentFixture>();

            // Assert
            Assert.IsTrue(fixtureType == typeof(ComponentFixture));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_type_throws_exception_when_manually_added()
        {
            // Act
            TypePool.AddType(null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_from_pool_from_parameterized_generic()
        {
            // Arrange
            var fixture = new ComponentFixture();

            // Act
            Type fixtureType = TypePool.GetType(fixture);

            // Assert
            Assert.IsTrue(fixtureType == typeof(ComponentFixture));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_types_using_predicate()
        {
            // Arrange
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => TypePool.GetPropertiesForType(type));

            // Act
            IEnumerable<Type> typeCollection = TypePool.GetTypes(t => t.IsSubclassOf(typeof(ComponentFixture)));

            // Assert
            Assert.IsTrue(typeCollection.Any());
            Assert.IsTrue(typeCollection.All(t => t.IsSubclassOf(typeof(ComponentFixture))));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_returns_cached_type()
        {
            // Arrange
            Type originalType = TypePool.GetType<TypePoolFixture>();

            // Act
            Type cachedType = TypePool.GetType<TypePoolFixture>();

            // Assert
            Assert.AreEqual(typeof(TypePoolFixture), cachedType);
            Assert.IsTrue(object.ReferenceEquals(originalType, cachedType), "The cached type was not returned.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_from_instance_returns_a_type()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            Type cachedType = TypePool.GetType(fixture);

            // Assert
            Assert.AreEqual(typeof(TypePoolFixture), cachedType);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_from_instance_returns_cached_type()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            Type originalType = TypePool.GetType(fixture);

            // Act
            Type cachedType = TypePool.GetType(fixture);

            // Assert
            Assert.AreEqual(typeof(TypePoolFixture), cachedType);
            Assert.IsTrue(object.ReferenceEquals(originalType, cachedType), "The cached type was not returned.");
        }
        #endregion

        #region GetProperties tests
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_for_type_without_cache()
        {
            // Act
            var properties = TypePool.GetPropertiesForType<TypePoolFixture>();

            // Assert
            Assert.IsTrue(properties.Count() == 11, "The number of properties expected back did not match the number of properties returned for the fixture.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_for_type_with_existing_cache()
        {
            //Arrange
            // Creates an initial cache for us
            var properties = TypePool.GetPropertiesForType<TypePoolFixture>();

            // Act
            properties = TypePool.GetPropertiesForType<TypePoolFixture>();

            // Assert
            Assert.IsTrue(properties.Count() == 11, "The number of properties expected back did not match the number of properties returned for the fixture.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_for_type_without_cache_using_predicate()
        {
            // Arrange
            var timer = new Stopwatch();
            timer.Start();

            // Act
            var properties =
                TypePool.GetPropertiesForType<TypePoolFixture>(
                    info => Attribute.IsDefined(info, typeof(AttributeFixture)));
            timer.Stop();

            Debug.WriteLine(timer.Elapsed.TotalMilliseconds);

            // Assert
            Assert.IsTrue(properties.Count() == 1);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_for_type_with_cache_using_predicate()
        {
            // Arrange
            Get_properties_for_type_without_cache();

            // Act
            PropertyInfo property = TypePool
                .GetProperty<TypePoolFixture>(info => Attribute.IsDefined(info, typeof(AttributeFixture)));

            // Assert
            Assert.IsNotNull(property);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_from_instance()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<PropertyInfo> properties = TypePool.GetPropertiesForType(fixture);

            // Assert
            Assert.IsNotNull(properties);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_properties_from_null_instance_throws_exception()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<PropertyInfo> properties = TypePool.GetPropertiesForType<TypePoolFixture>(null, null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_from_instance_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<PropertyInfo> properties = 
                TypePool.GetPropertiesForType(
                    fixture, 
                    p => p.Name == fixture.GetPropertyName(pr => pr.IsEnabled));

            // Assert
            Assert.IsNotNull(properties);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Remove_type_from_pool_without_cache()
        {
            // Act
            TypePool.ClearTypeFromPool<ComponentFixture>();

            // Assert
            Assert.IsFalse(TypePool.HasTypeInCache<ComponentFixture>());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Remove_type_from_pool_with_existing_cache()
        {
            // Arrange
            TypePool.GetPropertiesForType(typeof(ComponentFixture));

            // Act
            TypePool.ClearTypeFromPool<ComponentFixture>();

            // Assert
            Assert.IsFalse(TypePool.HasTypeInCache<ComponentFixture>());
        }

        #endregion

        #region Performance Tests
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_without_cache()
        {
            // Arrange
            // Pre-load all of the Domain Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => TypePool.GetPropertiesForType(type));
            var times = new List<double>();
            const int _iterations = 1000;

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                // Remove PayItem so we can test it not existing in the Pool.
                TypePool.ClearTypeFromPool<TypePoolFixture>();

                // Act
                var timer = new Stopwatch();
                timer.Start();
                IEnumerable<PropertyInfo> results =
                    TypePool.GetPropertiesForType<TypePoolFixture>();
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch an uncached collection of properties over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_with_cache()
        {
            // Arrange
            // Pre-load all of the Domain Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => TypePool.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                var timer = new Stopwatch();
                timer.Start();
                IEnumerable<PropertyInfo> results =
                    TypePool.GetPropertiesForType<TypePoolFixture>();
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch a cached collection of properties over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_without_cache_using_predicate()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => TypePool.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();


            // Act
            for (int count = 0; count < _iterations; count++)
            {
                TypePool.ClearTypeFromPool<TypePoolFixture>();
                var timer = new Stopwatch();
                timer.Start();
                IEnumerable<PropertyInfo> results =
                    TypePool.GetPropertiesForType<TypePoolFixture>(info => Attribute.IsDefined(info, typeof(AttributeFixture)));
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch an uncached collection of filtered properties over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_with_cache_using_predicate()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => TypePool.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            TypePool.GetAttributes(typeof(TypePoolFixture));

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                var timer = new Stopwatch();
                timer.Start();
                IEnumerable<PropertyInfo> results =
                    TypePool.GetPropertiesForType<TypePoolFixture>(info => Attribute.IsDefined(info, typeof(AttributeFixture)));
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch a cached collection of filtered properties over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_without_cache_from_large_cache_pool()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => TypePool.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                // Remove PayItem so we can test it not existing in the Pool.
                TypePool.ClearTypeFromPool<TypePoolFixture>();

                var timer = new Stopwatch();
                timer.Start();
                var results = TypePool.GetPropertiesForType<TypePoolFixture>();
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch an uncached collection properties from a large pool over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_with_cache_from_large_cache_pool()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => TypePool.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                var timer = new Stopwatch();
                timer.Start();
                var results = TypePool.GetPropertiesForType<TypePoolFixture>();
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch a cached collection properties from a large pool over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_without_cache_from_large_cache_pool_using_predicate()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => TypePool.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                TypePool.ClearTypeFromPool<TypePoolFixture>();
                var timer = new Stopwatch();
                timer.Start();
                var results = TypePool
                    .GetProperty<TypePoolFixture>(
                        property => Attribute.IsDefined(property, typeof(AttributeFixture)));
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch an uncached collection of filtered properties from a large pool over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_with_cache_from_large_cache_pool_using_predicate()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => TypePool.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                var timer = new Stopwatch();
                timer.Start();
                var results =
                    TypePool.GetPropertiesForType<TypePoolFixture>(
                    property => Attribute.IsDefined(property, typeof(AttributeFixture)));
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch a cached collection of filtered properties from a large pool over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }
        #endregion

        #region GetAttribute tests
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_for_type()
        {
            // Arrange
            var type = typeof(TypePoolFixture);

            // Act
            IEnumerable<Attribute> attributes = TypePool.GetAttributes(type);

            // Assert
            Assert.IsTrue(attributes.Count() == 3);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_for_with_property_info()
        {
            // Arrange
            var type = typeof(TypePoolFixture);
            var fixture = new TypePoolFixture();
            var property = TypePool.GetProperty(type, p => p.Name == nameof(fixture.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes = TypePool.GetAttributes(type, property);

            // Assert
            Assert.IsTrue(attributes.Count() == 3);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_from_generic_type()
        {
            // Act
            IEnumerable<Attribute> attributes = TypePool.GetAttributes<AttributeFixture, TypePoolFixture>();

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_from_generic_type_from_property_info()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            PropertyInfo property = typeof(TypePoolFixture).GetProperty(nameof(fixture.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes = 
                TypePool.GetAttributes<AttributeFixture, TypePoolFixture>(property);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_from_generic_type_from_property_info_lacking_attribute_returns_null()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            PropertyInfo property = typeof(TypePoolFixture).GetProperty(nameof(fixture.LongFixture));

            // Act
            IEnumerable<Attribute> attributes = 
                TypePool.GetAttributes<AttributeFixture, TypePoolFixture>(property);

            // Assert
            Assert.IsTrue(attributes.Count() == 0);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_from_generic_type_from_property_info_and_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            PropertyInfo property = typeof(TypePoolFixture).GetProperty(nameof(fixture.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes = 
                TypePool.GetAttributes<AttributeFixture, TypePoolFixture>
                    (property, attribute => attribute.GetType() == typeof(AttributeFixture));

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_from_generic_type_with_predicate()
        {
            // Act
            IEnumerable<Attribute> attributes = 
                TypePool.GetAttributes<AttributeFixture, TypePoolFixture>(
                    null, attribute => attribute.GetType() == typeof(AttributeFixture));

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_of_T_from_type()
        {
            // Act
            IEnumerable<Attribute> attributes = TypePool
                .GetAttributes<AttributeFixture>(typeof(TypePoolFixture));

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_of_T_from_property_on_type()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            PropertyInfo property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes = TypePool
                .GetAttributes<AttributeFixture>(typeof(TypePoolFixture), property);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attributes_from_instance()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            
            // Act
            IEnumerable<Attribute> attributes = 
                TypePool.GetAttributes<AttributeFixture, TypePoolFixture>(fixture);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attributes_from_instance_property()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes =
                TypePool.GetAttributes<AttributeFixture, TypePoolFixture>(fixture, property);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attributes_from_instance_property_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes =
                TypePool.GetAttributes<AttributeFixture, TypePoolFixture>(
                    fixture, property, attribute => attribute is AttributeFixture);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attributes_from_instance_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            
            // Act
            IEnumerable<Attribute> attributes =
                TypePool.GetAttributes<AttributeFixture, TypePoolFixture>(
                    fixture, null, attribute => attribute is AttributeFixture);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attributes_from_null_instance()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<Attribute> attributes =
                TypePool.GetAttributes<AttributeFixture, TypePoolFixture>(null, null, null);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attribute_from_type()
        {
            // Act
            Attribute attribute =
                TypePool.GetAttribute(typeof(TypePoolFixture));

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attribute_from_property_on_type()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            Attribute attribute =
                TypePool.GetAttribute(typeof(TypePoolFixture), property);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attribute_from_property_on_type_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            Attribute attribute =
                TypePool.GetAttribute(typeof(TypePoolFixture), property, a => a is AttributeFixture);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attribute_on_type_with_predicate()
        {
            // Act
            Attribute attribute =
                TypePool.GetAttribute(typeof(TypePoolFixture), null, a => a is AttributeFixture);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_attribute_on_null_type()
        {
            // Act
            Attribute attribute = TypePool.GetAttribute(null, null, null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_of_T_from_property_on_type_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            PropertyInfo property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes = 
                TypePool.GetAttributes<AttributeFixture>(
                    typeof(TypePoolFixture), property, attribute => attribute is AttributeFixture);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_type()
        {
            // Act
            AttributeFixture attribute =
                TypePool.GetAttribute<AttributeFixture>(typeof(TypePoolFixture));

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_property_on_type()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                TypePool.GetAttribute<AttributeFixture>(typeof(TypePoolFixture), property);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_property_on_type_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                TypePool.GetAttribute<AttributeFixture>(typeof(TypePoolFixture), property, a => a.IsEnabled);

            // Assert
            Assert.IsNotNull(attribute);
            Assert.IsTrue(attribute.IsEnabled);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_on_type_with_predicate()
        {
            // Act
            AttributeFixture attribute =
                TypePool.GetAttribute<AttributeFixture>(typeof(TypePoolFixture), null, a => a.IsEnabled);

            // Assert
            Assert.IsNotNull(attribute);
            Assert.IsTrue(attribute.IsEnabled);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_generic_attribute_on_null_type()
        {
            // Act
            AttributeFixture attribute = TypePool.GetAttribute<AttributeFixture>(null, null, null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_instance()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            AttributeFixture attribute =
                TypePool.GetAttribute<AttributeFixture, TypePoolFixture>();

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_instance_property()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                TypePool.GetAttribute<AttributeFixture, TypePoolFixture>(property);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_single_attribute_from_instance_property()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                TypePool.GetAttribute<AttributeFixture, TypePoolFixture>(property, fixture);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_single_attribute_from_instance_property_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                TypePool.GetAttribute<AttributeFixture, TypePoolFixture>(
                    property, 
                    fixture,
                    a => a.IsEnabled);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_null_instance_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                TypePool.GetAttribute<AttributeFixture, TypePoolFixture>(
                    property, null, a => a.IsEnabled);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_null_property_and_null_instance_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                TypePool.GetAttribute<AttributeFixture, TypePoolFixture>(
                    null, null, a => a.IsEnabled);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_null_instance()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<Attribute> attributes =
                TypePool.GetAttributes<AttributeFixture, TypePoolFixture>(null, null, null);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_of_T_from_type_with_predicate()
        {
            // Act
            IEnumerable<Attribute> attributes = 
                TypePool.GetAttributes<AttributeFixture>(
                    typeof(TypePoolFixture), null, attribute => attribute is AttributeFixture);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_with_property_info_using_predicate()
        {
            // Arrange
            var type = typeof(TypePoolFixture);
            var fixture = new TypePoolFixture();
            var property = TypePool.GetProperty(type, p => p.Name == nameof(fixture.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes = TypePool.GetAttributes(
                type,
                property,
                attribute => attribute.GetType() == typeof(AttributeFixture));

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
        }



        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attribute_with_cache()
        {
            // Arrange
            var type = typeof(TypePoolFixture);
            var fixture = new TypePoolFixture();
            var property = TypePool.GetProperty(type, p => p.Name == nameof(fixture.PropertyWithAttribute));
            IEnumerable<Attribute> attributes = TypePool.GetAttributes(type, property);

            // Act

            // Assert
            Assert.IsTrue(attributes.Count() == 3);
        }
        #endregion
    }
}
