namespace ProductionExamples;

public interface ITemplateDataProvider
{
    DIToolTemplate GetTemplateInActiveContext();
    DIToolTemplate GetTemplateInSpecifiedOrActiveContext(string contextKey);
    bool IsTemplate(string contextKey);
}
