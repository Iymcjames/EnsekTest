import React, { useState } from "react";
import axios from "axios";

const FileUpload = () => {
  const [file, setFile] = useState<File | null>(null);
  const [message, setMessage] = useState<string>("");

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      setFile(event.target.files[0]);
    }
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!file) {
      setMessage("Please upload a file.");
      return;
    }

    const formData = new FormData();
    formData.append("file", file);

    try {
      const response = await axios.post(
        "http://localhost:5216/api/meter-reading-uploads",
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      console.log("response=============", response);
      setMessage(
        `${response?.data?.message}
         Processed records: ${response?.data?.processedRecordCount}
         Unprocessed records: ${response?.data?.unprocessedRecordCount}
         Validation error: ${response?.data?.validationError}`
      );
    } catch (error) {
      setMessage(
        `Error: ${
          (error as any).response
            ? (error as any).response.data
            : (error as any).message
        }`
      );
    }
  };

  return (
    <div>
      <h1>Upload Meter Readings</h1>
      <form onSubmit={handleSubmit}>
        <input type="file" onChange={handleFileChange} />
        <button type="submit">Upload</button>
      </form>
      {message && (
        <p>
          {message.split("\n").map((line, index) => (
            <React.Fragment key={index}>
              {line}
              <br />
            </React.Fragment>
          ))}
        </p>
      )}
    </div>
  );
};

export default FileUpload;
