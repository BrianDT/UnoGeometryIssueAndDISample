# UnoGeometryIssueAndDISample
Demonstrates a Uno WASM issue in Geometry shape binding. Also provides a sample of the use of different types of DI containers.

The sample contains a lot of projects as it is also designed to demonstrate enforcement of decoupling through project structures.
Inspection of the layers will show that they only interact with other layers via there interfaces and this is enforced by there being no inter-project references between Views and ViewModels.
The same would apply between ViewModels and Services and potentially between ViewModels and Models though this depends on persistence mechanism and how that is mapped.