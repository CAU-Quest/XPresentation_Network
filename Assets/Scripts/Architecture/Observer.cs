using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IObserver
{
    void ObserverChangeSlide(int slide);
}

public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObserversChangeSlide(int slide);
}