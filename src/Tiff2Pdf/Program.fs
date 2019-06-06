// Learn more about F# at http://fsharp.org

open System
open iTextSharp.text
open iTextSharp.text.pdf
open System.IO
open System.Drawing
open System.Drawing.Imaging

[<EntryPoint>]
let main argv =
    let input = argv.[0]
    let output = Path.ChangeExtension(input, ".output.pdf")

    printfn "= Process %A" input

    let document = new Document(PageSize.A4)

    let writer = PdfWriter.GetInstance(document, new FileStream(output, FileMode.Create))
    let bitmap = new Bitmap(input);
    let total = bitmap.GetFrameCount(FrameDimension.Page)

    document.Open()

    let content = writer.DirectContent;
    for i in 0..total - 1 do
        bitmap.SelectActiveFrame(FrameDimension.Page, i)  |> ignore
        let img = iTextSharp.text.Image.GetInstance(bitmap, ImageFormat.Tiff)
        img.SetAbsolutePosition(0.f,0.f)
        img.ScaleAbsoluteHeight(document.PageSize.Height)
        img.ScaleAbsoluteWidth(document.PageSize.Width)
        content.AddImage(img)
        document.NewPage() |> ignore

        printfn " Add Page %A" i

    document.Close()
    printfn "= Write %A " output
    0