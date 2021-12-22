#r "nuget:Microsoft.ML"
#r "nuget:Microsoft.ML.OnnxRuntime"
#r "nuget:Microsoft.ML.OnnxTransformer"
#r "nuget:Microsoft.ML.ImageAnalytics"
#r "nuget:System.Drawing.Common"

open System.IO
open System.Drawing
open Microsoft.ML
open Microsoft.ML.Data

let onnxFilePath = Path.Join(__SOURCE_DIRECTORY__,"mosaic-8.onnx")

let ctx = new MLContext()

let data = 
    seq {
        {|  ImagePath = Path.Join(__SOURCE_DIRECTORY__,"forest.jpg")  |}
    }

let idv = ctx.Data.LoadFromEnumerable(data)

let pipeline = 
    EstimatorChain()
        .Append(ctx.Transforms.LoadImages("Image",null,"ImagePath"))
        .Append(ctx.Transforms.ResizeImages("ResizedImage",224,224,"Image"))
        .Append(ctx.Transforms.ExtractPixels("input1","ResizedImage"))
        .Append(ctx.Transforms.ApplyOnnxModel("output1","input1",onnxFilePath))
        .Append(ctx.Transforms.ConvertToImage(224,224,"TransformedImage","output1"))

let transformedData = pipeline.Fit(idv).Transform(idv)

let images = transformedData.GetColumn<Bitmap>("TransformedImage")

let firstImage = images |> Seq.head

firstImage.Save("transformed.jpg")