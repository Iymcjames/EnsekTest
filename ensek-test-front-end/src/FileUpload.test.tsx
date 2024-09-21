import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import FileUpload from "./FileUpload";
import axios from "axios";
import { AxiosResponse } from "axios";

jest.mock("axios");

const mockedAxios = axios as jest.Mocked<typeof axios>;

test("renders upload form", () => {
  render(<FileUpload />);
  expect(screen.getByText(/Upload Meter Readings/i)).toBeInTheDocument();
});

test("displays error message when no file is uploaded", async () => {
  render(<FileUpload />);
  fireEvent.click(screen.getByText(/Upload/i));
  expect(await screen.findByText(/Please upload a file/i)).toBeInTheDocument();
});

test("displays success message on successful upload", async () => {
  const mockResponse: any = {
    data: { Message: "Meter readings imported." },
    status: 200,
    statusText: "OK",
    headers: {},
  };

  mockedAxios.post.mockResolvedValue(mockResponse);

  render(<FileUpload />);
  const file = new File(["(⌐□_□)"], "sample.csv", { type: "text/csv" });
  fireEvent.change(screen.getByLabelText(/file/i), {
    target: { files: [file] },
  });
  fireEvent.click(screen.getByText(/Upload/i));

  expect(await screen.findByText(/Success:/i)).toBeInTheDocument();
});
