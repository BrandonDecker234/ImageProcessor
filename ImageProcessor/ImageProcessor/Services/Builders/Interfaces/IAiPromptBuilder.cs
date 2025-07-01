using ImageProcessor.Models;
using ImageProcessor.Models.LLMConfigs;

namespace ImageProcessor.Services.Builders.Interfaces;

public interface IAiPromptBuilder
{
    // TODO: Figure out a clean way to pass in and manage prompts.
    void BuildSystemPrompt(LlmRequestConfigs llmRequestConfigs);
    void BuildUserPrompt(LlmRequestConfigs llmRequestConfigs, string imageUrl);
}