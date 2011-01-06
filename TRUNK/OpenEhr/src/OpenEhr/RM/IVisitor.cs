using OpenEhr.RM.Composition.Content.Navigation;
using OpenEhr.RM.Composition.Content.Entry;
using OpenEhr.RM.DataStructures.History;
using OpenEhr.RM.DataStructures.ItemStructure.Representation;

namespace OpenEhr.RM.Impl
{
    public interface IVisitor
    {
        void VisitComposition(Composition.Composition composition);
        void VisitEventContext(Composition.EventContext context);
        void VisitSection(Section section);
        void VisitObservation(Observation observation);
        void VisitEvaluation(Evaluation evaluation);
        void VisitAdminEntry(AdminEntry adminEntry);
        void VisitInstruction(Instruction instruction);
        void VisitAction(Action action);
        void VisitActivity(Activity activity);
        //void VisitHistory<T>(History<T> history);
        //void VisitPointEvent<T>(PointEvent<T> @event);
        //void VisitIntervalEvant<T>(IntervalEvent<T> @event);
        //void VisitCluster(Cluster cluster);
        //void VisitElement(Element element);
    }
}
