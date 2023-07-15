using Events;

public static class CharacterEvents
{
    public static EventHolder<ConveyorBeltObject> StartedGrabbingFruit = new ();
    public static EventHolder<ConveyorBeltObject> TouchedBeforeGrabbedFruit = new ();
    public static EventHolder<ConveyorBeltObject> GrabbedFruit = new ();
}