using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject
{
    public void Add(IObserver observer);

    public void Remove(IObserver observer);

    public void Notify(ISubject subject);
}

public interface IObserver
{
    public void Notify();
}

public class ObserverPattern : MonoBehaviour
{
    
}
