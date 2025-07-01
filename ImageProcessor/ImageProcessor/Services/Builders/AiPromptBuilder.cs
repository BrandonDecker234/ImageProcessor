using ImageProcessor.Models;
using ImageProcessor.Models.LLMConfigs;
using ImageProcessor.Services.Builders.Interfaces;

namespace ImageProcessor.Services.Builders;

public class AiPromptBuilder : IAiPromptBuilder
{
    public void BuildSystemPrompt(LlmRequestConfigs llmRequestConfigs)
    {
        llmRequestConfigs.Messages.Add(
            new ChatMessage
            {
                Role = "system",
                Content = [
                    new Content
                    {
                        Type = "text",
                        Text = """
                               You are an image‐analysis assistant. For every request, output **only** valid JSON that matches **exactly** the following schema. Do not include any extra keys, comments, or prose.
                               
                               Desired JSON schema:
                               ```json
                               {
                                 "type": "object",
                                 "properties": {
                                   "tags": {
                                     "type": "array",
                                     "items": {
                                       "type": "object",
                                       "properties": {
                                         "name":       { "type": "string"  },
                                         "confidence": { "type": "number"  }
                                       },
                                       "required": ["name", "confidence"],
                                       "additionalProperties": false
                                     }
                                   }
                                 },
                                 "required": ["tags"],
                                 "additionalProperties": false
                               }
                               """
                    }
                ]
            }
        );
    }

    public void BuildUserPrompt(LlmRequestConfigs llmRequestConfigs, string imageUrl)
    {
        llmRequestConfigs.Messages.Add(
            new ChatMessage
            {
                Role = "user",
                Content = [
                    new Content
                    {
                        Type = "image_url",
                        ImageUrl = new ImageUrl
                        {
                            Url = imageUrl
                        },
                        Text = "Image to be analyzed."
                    },
                    new Content
                    {
                        Type = "text",
                        Text = "Analyze this image."
                    }
                ]
            }
        );
    }
}