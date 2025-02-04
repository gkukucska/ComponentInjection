# ComponentGenerators
The ComponentGenerators package is a set of source generators, analyzers and related code fixes that is designed to extend and reduce boilerplate code related to dependency injection.
The package builds upon the ```IHostApplicationBuilder``` interface and the ```IOptions<T>``` pattern.

## Basic Usage

The source generators create some attributes that can be used to mark classes for generating dependency injection logic.

The following list contains all attributes that can be attached to a class:
- ```ServiceAttribute```: Marks the class as a simple service
- ```KeyedServiceAttribute```: Marks the class as a keyed service
- ```ComponentAttribute```: Marks the class as a keyed service with a dynamic key and a respective configuration class
- ```KeylessComponentAttribute```: Marks the class as a keyed service with a static key and a respective configuration class
- ```HostedServiceAttribute```: Marks the class as a hosted service, implementing the ```IHostedService``` interface
- ```ApplicationAttribute```: Marks the class as the main application (```Program```) class
- ```AliasAttribute```: Marks a constructor parameter to be dynamically resolved based on a generated configuration value
- ```AliasCollectionAttribute```: Marks a constructor parameter that is a type of ```IEnumarable``` to be dynamically resolved based on a generated configuration value
- ```OptionalAttribute```: Marks a constructor parameter as an optional dependency

Details of the usage for each attribute can be found below. 

### ```ServiceAttribute```

This attribute, when attached to a class, will mark the class as a simple service and related code for registration will be generated. \
Dependencies for given class will be resolved based on its constructor (if multiple constructors are present, the one with the most parameters are used), including keyed services marked with the ```FromKeyedServicesAttribute```. 

#### Required parameters:

- ```ServiceLifetime lifetime```: Declares the lifetime of the registered service (either ```Singleton```,```Scoped``` or ```Transient```)
- ```params Type[] implementationTypeCollection```: Declares the service types that the current class should be used as an implementation. Note that each implementation will refer to the same factory method, thus if a service is registered as a ```Singleton```, requests for different interfaces will return the same instance.

#### Example of usage

<details>
<summary>Service registration</summary>

```csharp

public interface IService 
{
    //interface details    
}

[Service(ServiceLifetime.Singleton, typeof(IService))]
public class Service : IService
{
    //implementation details
}

```

</details>

<details>
<summary>Service registration implementing multiple interfaces</summary>

```csharp

public interface IService 
{
    //interface details    
}

public interface IOtherService 
{
    //interface details    
}

[Service(ServiceLifetime.Singleton, typeof(IService), typeof(IOtherService))]
public class Service : IService, IOtherService
{
    //implementation details
}

```

</details>


### ```KeyedServiceAttribute```

This attribute, when attached to a class, will mark the class as a keyed service and related code for registration will be generated. \
Dependencies for given class will be resolved based on its constructor (if multiple constructors are present, the one with the most parameters are used), including keyed services marked with the ```FromKeyedServicesAttribute```.

#### Required parameters:

- ```string serviceKey```: Key of the service
- ```ServiceLifetime lifetime```: Declares the lifetime of the registered service (either ```Singleton```,```Scoped``` or ```Transient```)
- ```params Type[] implementationTypeCollection```: Declares the service types that the current class should be used as an implementation. Note that each implementation will refer to the same factory method, thus if a service is registered as a ```Singleton```, requests for different interfaces will return the same instance.

#### Example of usage

<details>
<summary>Keyed service registration</summary>

```csharp

public interface IService 
{
    //interface details    
}

[KeyedService(nameof(KeyedService), ServiceLifetime.Singleton, typeof(IService))]
public class KeyedService : IService
{
    //implementation details
}

```

</details>

<details>
<summary>Keyed service registration implementing multiple interfaces</summary>

```csharp

public interface IService 
{
    //interface details    
}

public interface IOtherService 
{
    //interface details    
}

[Service(nameof(KeyedService), ServiceLifetime.Singleton, typeof(IService), typeof(IOtherService))]
public class KeyedService : IService, IOtherService
{
    //implementation details
}

```

</details>


### ```HostedServiceAttribute```

This attribute, when attached to a class, will mark the class as a hosted service and related code for registration will be generated. \
Dependencies for given class will be resolved based on its constructor (if multiple constructors are present, the one with the most parameters are used), including keyed services marked with the ```FromKeyedServicesAttribute```.

#### Required parameters:

- ```Type optionType```: Type of the option class

#### Example of usage

<details>
<summary>Hosted service registration</summary>

```csharp

[HostedService(typeof(HostedServiceOptions))]
public class HostedService : IHostedService
{
    //implementation details
}

public class HostedServiceOptions
{
    //implementation details
}

```

</details>

### ```ComponentAttribute```

This attribute, when attached to a class, will mark the class as a component and related code for registration will be generated. 

For more information refer to the Components and Aliases section. 

Dependencies for given class will be resolved based on its constructor (if multiple constructors are present, the one with the most parameters are used), including keyed services marked with the ```FromKeyedServicesAttribute```, ```AliasAttribute```, ```AliasCollectionAttribute```.

When resolving, if a constructor parameter is defined with the type of the option class, the respective option class will be injected.

#### Required parameters:

- ```Type optionType```: Type of the option class
- ```ServiceLifetime lifetime```: Declares the lifetime of the registered service (either ```Singleton```,```Scoped``` or ```Transient```)
- ```params Type[] implementationTypeCollection```: Declares the service types that the current class should be used as an implementation. Note that each implementation will refer to the same factory method, thus if a service is registered as a ```Singleton```, requests for different interfaces will return the same instance.

#### Example of usage

<details>
<summary>Component registration</summary>

```csharp

public interface IComponent 
{
    //interface details    
}

[Component(typeof(ComponentOptions), ServiceLifetime.Singleton, typeof(IComponent))]
public class ComponentService : IComponent
{
    //implementation details
}

public partial class ComponentOptions
{
        
}

```

</details>

### ```KeylessComponentAttribute```

This attribute, when attached to a class, will mark the class as a keyless component and related code for registration will be generated.

Keyless components are special type of components, that are not registered dynamically, but only once as a keyed service with the fully qualified class name as the key (options class will map to the same section).

For more information refer to the Components and Aliases section.

Dependencies for given class will be resolved based on its constructor (if multiple constructors are present, the one with the most parameters are used), including keyed services marked with the ```FromKeyedServicesAttribute```, ```AliasAttribute```, ```AliasCollectionAttribute```.

When resolving, if a constructor parameter is defined with the type of the option class, the respective option class will be injected.

#### Required parameters:

- ```Type optionType```: Type of the option class
- ```ServiceLifetime lifetime```: Declares the lifetime of the registered service (either ```Singleton```,```Scoped``` or ```Transient```)
- ```params Type[] implementationTypeCollection```: Declares the service types that the current class should be used as an implementation. Note that each implementation will refer to the same factory method, thus if a service is registered as a ```Singleton```, requests for different interfaces will return the same instance.

#### Example of usage

<details>
<summary>Keyless Component registration</summary>

```csharp

public interface IKeylessComponent 
{
    //interface details    
}

[KeylessComponent(typeof(KeylessComponentOptions), ServiceLifetime.Singleton, typeof(IKeylessComponent))]
public class KeylessComponentService : IKeylessComponent
{
    //implementation details
}

public partial class KeylessComponentOptions
{
        
}

```

</details>

### ```ApplicationAttribute```

The ```ApplicationAttribute```, when attached to a class, will generate two extension methods to the ```IHostApplicationBuilder``` interface:
- ```InstallComponents()```: Installs all Services, Keyed services, Components and Keyless components that are in any directly reference assemblies.
- ```ValidateComponents()```: Runs validation logic, validating Components and Keyless components that are in any directly reference assemblies.

Note: only classes in directly referenced assemblies are taken into account, if an assembly is not directly referenced, the contained classes will not be included in the aforementioned methods.

If Components are used, a component section within the configuration is required, this section should contain the aliases and the related services (with fully qualified classnames) as key-value pairs.

### ```AliasAttribute```

The ```AliasAttribute```, when attached to a constructor parameter, will indicate that the resolution of the service will be done through a keyed service, with the service key contained as a string in the associated option class.
The generated property within the option class will have the same name as the constructor parameter with the first letter capitalized.

The ```AliasAttribute``` can only resolve, when it is attached to the constructor parameter of a class with either ```ComponentAttribute``` or ```KeylessComponentAttribute```.

Note: The value of the alias value (in the options class) must be a registered service with the component section of the configuration with a class that implements the requested service. 

For detailed examples please check the examples folder within the repository.

### ```AliasCollectionAttribute```

The ```AliasCollectionAttribute```, when attached to a constructor parameter, will indicate that the resolution of the service will be done by gathering keyed services, with the service keys contained as a ```List<string>``` in the associated option class.

The generated property within the option class will have the same name as the constructor parameter with the first letter capitalized.

The ```AliasCollectionAttribute``` can only resolve, when it is attached to the constructor parameter of a class with either ```ComponentAttribute``` or ```KeylessComponentAttribute```.

Note: All values of the alias collection (in the options class) must be a registered service with the component section of the configuration with a class that implements the requested service.

For detailed examples please check the examples folder within the repository.



### ```OptionalAttribute```

The ```OptionalAttribute```, when attached to a constructor parameter will mark the dependency as non-required and will result in the following effects:
- When resolving the dependency, the ```GetService``` method of the service provider will be called instead of the ```GetRequiredService```
- When generating validation logic, non-required services won't be checked

Note: The same effect can be achieved by using nullable reference types and declaring the constructor parameter nullable.

#### Example of usage

<details>
<summary>Optional dependency with attribute</summary>

```csharp

public interface IComponent 
{
    //interface details    
}

public interface IRequiredDependency
{
    //interface details     
}

public interface IOptionalDependency
{
    //interface details     
}

[Component(typeof(ComponentOptions), ServiceLifetime.Singleton, typeof(IComponent))]
public class ComponentService : IComponent
{
    public ComponentService(IRequiredDependecy required, [Optional]IOptionalDependency optional)
    {
    
    }
    //implementation details
}

public partial class ComponentOptions
{
        
}

```
</details>


<details>
<summary>Optional dependency with nullable reference</summary>

```csharp

public interface IComponent 
{
    //interface details    
}

public interface IRequiredDependency
{
    //interface details     
}

public interface IOptionalDependency
{
    //interface details     
}

[Component(typeof(ComponentOptions), ServiceLifetime.Singleton, typeof(IComponent))]
public class ComponentService : IComponent
{
    public ComponentService(IRequiredDependecy required, IOptionalDependency? optional)
    {
    
    }
    //implementation details
}

public partial class ComponentOptions
{
        
}

```
</details>

## Components and Aliases

The ```ComponentGenerator``` package introduces two concepts that extend the functionality of dependency injection: Components and Aliases.

In short, Components are special kind of services that can be injected into another component through an Alias (a.k.a. dynamic service key).

These concepts extend DI logic with the following points:
- Configurable application structure
- Registration of multiple instances of the same service with different configuration
- Easy replacement of a service in all services

### Component

A Component is a keyed service, with a respective configuration class.
Additionally, registration of components into the DI container is not done in a static way, but based on specific configuration entries.
An application with the ```ApplicationAttribute``` attached will have a specific configuration section that contains key-value pairs, with the keys containing the service keys (a.k.a. Alias) and values containing the types .
During the registration process, these values will be used for registering components, thus changing configuration values will change the registered. 

### Alias

Aliases are (in short) the counterpart of Components, as during the resolution of dependencies, aliased constructor parameters are resolved through a configuration value.
A constructor parameter is considered aliased if either the ```AliasAttribute``` or ```AliasCollectionAttribute``` is attached to it.

In case of a parameter with ```AliasAttribute``` attached, a configuration property will be generated (of type string) and used to resolve the service as a keyed service (with the configuration value as the service key).

In case of a parameter with ```AliasCollectionAttribute``` attached, a configuration property will be generated and used to resolve the service as a keyed service (with the configuration value as the service key).

#### Nullability

If the constructor argument is a nullable reference type, or marked with the ```OptionalAttribute```, it will be handled as an optional service and will be resolver as a non-required service.

## Validation

Since the ComponentGenerator package mixes configuration values with dependency injection logic, breaking an application by changing configuration values becomes relatively simple. 
In case of missing (or broken) dependencies that are not instantiated during startup, may cause unexpected breaking of an application well into its runtime.
To avoid such scenarios, validation logic is dynamically generated as an extension method for ```IHostApplicationBuilder``` alongside the registration logic.

The validation logic goes through all defined components and checks for the following:
- Are all dependent services and keyed services are registered
- Are all aliased dependencies are registered

Checks are performed on the type, and key of registered services.

The validation logic does not check for the dependencies of registered services and keyed services, since these are registered statically and does not depend on configuration values.

## Examples

Examples with detailed documentation cna be found in the examples folder next to the source code.