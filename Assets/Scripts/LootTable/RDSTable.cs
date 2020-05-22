using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RDSTable : IRDSTable
{
    public int DropCount { get; set; }
    private List<IRDSObject> contents = null;
    public IEnumerable<IRDSObject> Contents { get { return contents; } }
    public float DropProbability { get; set; }
    public bool UniqueDrop { get; set; }
    public bool DropAlways { get; set; }
    public bool DropEnabled { get; set; }
    public RDSTable Table { get; set; }

    public event EventHandler EvaluatePreResult;
    public event EventHandler HitResult;
    public event EventHandler EvaluatePostResult;

    List<IRDSObject> uniqueDrops;

    public RDSTable() : this(null, 1, 1, false, false, true) { }
    public RDSTable(IEnumerable<IRDSObject> _contents, int _count, float _probability) : this(_contents, _count, _probability, false, false, true) { }
    public RDSTable(IEnumerable<IRDSObject> _contents, int _count, float _probability, bool _unique, bool _always, bool _enabled)
    {
        if (_contents != null)
            contents = _contents.ToList();
        else
            ClearContents();

        DropCount = _count;
        DropProbability = _probability;
        UniqueDrop = _unique;
        DropAlways = _always;
        DropEnabled = _enabled;
    }

    #region Events
    public virtual void OnEvaluatePostResult(EventArgs e)
    {
        EvaluatePostResult?.Invoke(this, e);
    }
    public virtual void OnEvaluatePreResult(EventArgs e)
    {
        EvaluatePreResult?.Invoke(this, e);
    }
    public virtual void OnHitResult(EventArgs e)
    {
        HitResult?.Invoke(this, e);
    }
    #endregion

    #region Content Manipulation
    public virtual void AddEntry(IRDSObject _entry)
    {
        contents.Add(_entry);
        _entry.Table = this;
    }
    public virtual void AddEntry(IRDSObject _entry, float _probability)
    {
        contents.Add(_entry);
        _entry.DropProbability = _probability;
        _entry.Table = this;
    }
    public virtual void AddEntry(IRDSObject _entry, float _probability, bool _unique, bool _always, bool _enabled)
    {
        contents.Add(_entry);
        _entry.DropProbability = _probability;
        _entry.UniqueDrop = _unique;
        _entry.DropAlways = _always;
        _entry.DropEnabled = _enabled;
        _entry.Table = this;
    }

    public virtual void RemoveEntry(IRDSObject _entry)
    {
        contents.Remove(_entry);
        _entry.Table = null;
    }
    public virtual void RemoveEntry(int _index)
    {
        IRDSObject entry = contents[_index];
        entry.Table = null;
        contents.RemoveAt(_index);
    }

    public virtual void ClearContents()
    {
        contents = new List<IRDSObject>();
    }
    #endregion

    public virtual IEnumerable<IRDSObject> GetResult()
    {
        List<IRDSObject> result = new List<IRDSObject>();
        uniqueDrops = new List<IRDSObject>();

        foreach (IRDSObject item in Contents)
        {
            item.OnEvaluatePreResult(EventArgs.Empty);
        }

        foreach (IRDSObject item in Contents.Where(e => e.DropAlways && e.DropEnabled))
        {
            AddToResult(result, item);
        }

        int alwaysCount = Contents.Count(e => e.DropAlways && e.DropEnabled);
        int realDropCount = DropCount - alwaysCount;

        if(realDropCount > 0)
        {
            for (int dropCount = 0; dropCount < realDropCount; dropCount++)
            {
                IEnumerable<IRDSObject> dropables = Contents.Where(e => e.DropEnabled && !e.DropAlways);

                float hitValue = RDSRandom.GetRandomFloat(dropables.Sum(e => e.DropProbability));

                float runningValue = 0;
                foreach (IRDSObject item in dropables)
                {
                    runningValue += item.DropProbability;
                    if(hitValue < runningValue)
                    {
                        AddToResult(result, item);
                        break;
                    }
                }
                    
            }
        }

        ResultEventArgs rea = new ResultEventArgs(result);
        foreach (IRDSObject item in result)
        {
            item.OnEvaluatePostResult(rea);
        }
        return result;
    }

    private void AddToResult(List<IRDSObject> _result, IRDSObject _object)
    {
        if(!_object.UniqueDrop || !uniqueDrops.Contains(_object))
        {
            if (_object.UniqueDrop)
                uniqueDrops.Add(_object);

            if(!(_object is RDSNullValue))
            {
                if(_object is IRDSTable)
                {
                    _result.AddRange(((IRDSTable)_object).GetResult());
                }
                else
                {
                    IRDSObject adder = _object;
                    if(_object is IRDSObjectCreator)
                    {
                        adder = ((IRDSObjectCreator)_object).CreateInstance();
                    }
                    _result.Add(adder);
                    _object.OnHitResult(EventArgs.Empty);
                }
            }
            else
            {
                _object.OnHitResult(EventArgs.Empty);
            }
        }
    }

    public string ToString(int _indent)
    {
        string indent = "".PadRight(4 * _indent, ' ');

        StringBuilder builder = new StringBuilder();
        builder.AppendFormat(_indent + "(RDSTable){0} Entries: {1}, Probability: {2}, UAE: {3}{4}{5}{6}",
            this.GetType().Name, 
            contents.Count, 
            DropProbability, 
            (UniqueDrop ? "1" : "0"), 
            (DropAlways ? "1" : "0"), 
            (DropEnabled ? "1" : "0"), 
            (contents.Count > 0 ? "\r\n" : "")
        );

        foreach (IRDSObject item in contents)
        {
            builder.AppendLine(indent + item.ToString(_indent + 1));
        }
        return builder.ToString();
    }
}
