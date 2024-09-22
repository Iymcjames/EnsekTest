import React from "react";
import { render, fireEvent, waitFor, screen } from "@testing-library/react";
import FileUpload from "./FileUpload";

jest.mock("axios");
const axios = require("axios");

describe("FileUpload Component", () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it("displays error message on file upload failure", async () => {
    const mockError = { response: { data: "File upload failed!" } };

    axios.post = jest.fn().mockRejectedValue(mockError);

    render(<FileUpload />);

    const file = new File(["sample content"], "testfile.csv", {
      type: "text/csv",
    });
    const fileInput = screen.getByLabelText(
      "Choose a file"
    ) as HTMLInputElement;

    fireEvent.change(fileInput, { target: { files: [file] } });
    fireEvent.click(screen.getByText("Upload"));

    await waitFor(() => {
      expect(
        screen.getByText(/Processed records: undefined/)
      ).toBeInTheDocument();
    });
  });
});
