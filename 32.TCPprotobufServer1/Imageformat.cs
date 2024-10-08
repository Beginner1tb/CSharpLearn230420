// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: imageformat.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from imageformat.proto</summary>
public static partial class ImageformatReflection {

  #region Descriptor
  /// <summary>File descriptor for imageformat.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static ImageformatReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "ChFpbWFnZWZvcm1hdC5wcm90byJPCglJbWFnZURhdGESDQoFd2lkdGgYASAB",
          "KAUSDgoGaGVpZ2h0GAIgASgFEg4KBmZvcm1hdBgDIAEoCRITCgtwaXhlbF9h",
          "cnJheRgEIAEoDGIGcHJvdG8z"));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::ImageData), global::ImageData.Parser, new[]{ "Width", "Height", "Format", "PixelArray" }, null, null, null, null)
        }));
  }
  #endregion

}
#region Messages
[global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
public sealed partial class ImageData : pb::IMessage<ImageData>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    , pb::IBufferMessage
#endif
{
  private static readonly pb::MessageParser<ImageData> _parser = new pb::MessageParser<ImageData>(() => new ImageData());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pb::MessageParser<ImageData> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::ImageformatReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public ImageData() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public ImageData(ImageData other) : this() {
    width_ = other.width_;
    height_ = other.height_;
    format_ = other.format_;
    pixelArray_ = other.pixelArray_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public ImageData Clone() {
    return new ImageData(this);
  }

  /// <summary>Field number for the "width" field.</summary>
  public const int WidthFieldNumber = 1;
  private int width_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int Width {
    get { return width_; }
    set {
      width_ = value;
    }
  }

  /// <summary>Field number for the "height" field.</summary>
  public const int HeightFieldNumber = 2;
  private int height_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int Height {
    get { return height_; }
    set {
      height_ = value;
    }
  }

  /// <summary>Field number for the "format" field.</summary>
  public const int FormatFieldNumber = 3;
  private string format_ = "";
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public string Format {
    get { return format_; }
    set {
      format_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "pixel_array" field.</summary>
  public const int PixelArrayFieldNumber = 4;
  private pb::ByteString pixelArray_ = pb::ByteString.Empty;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public pb::ByteString PixelArray {
    get { return pixelArray_; }
    set {
      pixelArray_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override bool Equals(object other) {
    return Equals(other as ImageData);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool Equals(ImageData other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Width != other.Width) return false;
    if (Height != other.Height) return false;
    if (Format != other.Format) return false;
    if (PixelArray != other.PixelArray) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override int GetHashCode() {
    int hash = 1;
    if (Width != 0) hash ^= Width.GetHashCode();
    if (Height != 0) hash ^= Height.GetHashCode();
    if (Format.Length != 0) hash ^= Format.GetHashCode();
    if (PixelArray.Length != 0) hash ^= PixelArray.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void WriteTo(pb::CodedOutputStream output) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    output.WriteRawMessage(this);
  #else
    if (Width != 0) {
      output.WriteRawTag(8);
      output.WriteInt32(Width);
    }
    if (Height != 0) {
      output.WriteRawTag(16);
      output.WriteInt32(Height);
    }
    if (Format.Length != 0) {
      output.WriteRawTag(26);
      output.WriteString(Format);
    }
    if (PixelArray.Length != 0) {
      output.WriteRawTag(34);
      output.WriteBytes(PixelArray);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
    if (Width != 0) {
      output.WriteRawTag(8);
      output.WriteInt32(Width);
    }
    if (Height != 0) {
      output.WriteRawTag(16);
      output.WriteInt32(Height);
    }
    if (Format.Length != 0) {
      output.WriteRawTag(26);
      output.WriteString(Format);
    }
    if (PixelArray.Length != 0) {
      output.WriteRawTag(34);
      output.WriteBytes(PixelArray);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(ref output);
    }
  }
  #endif

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int CalculateSize() {
    int size = 0;
    if (Width != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Width);
    }
    if (Height != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Height);
    }
    if (Format.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Format);
    }
    if (PixelArray.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeBytesSize(PixelArray);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(ImageData other) {
    if (other == null) {
      return;
    }
    if (other.Width != 0) {
      Width = other.Width;
    }
    if (other.Height != 0) {
      Height = other.Height;
    }
    if (other.Format.Length != 0) {
      Format = other.Format;
    }
    if (other.PixelArray.Length != 0) {
      PixelArray = other.PixelArray;
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(pb::CodedInputStream input) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    input.ReadRawMessage(this);
  #else
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
    if ((tag & 7) == 4) {
      // Abort on any end group tag.
      return;
    }
    switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 8: {
          Width = input.ReadInt32();
          break;
        }
        case 16: {
          Height = input.ReadInt32();
          break;
        }
        case 26: {
          Format = input.ReadString();
          break;
        }
        case 34: {
          PixelArray = input.ReadBytes();
          break;
        }
      }
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
    if ((tag & 7) == 4) {
      // Abort on any end group tag.
      return;
    }
    switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
          break;
        case 8: {
          Width = input.ReadInt32();
          break;
        }
        case 16: {
          Height = input.ReadInt32();
          break;
        }
        case 26: {
          Format = input.ReadString();
          break;
        }
        case 34: {
          PixelArray = input.ReadBytes();
          break;
        }
      }
    }
  }
  #endif

}

#endregion


#endregion Designer generated code
