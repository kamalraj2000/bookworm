import { ChangeEvent } from 'react';
import { InputGroup, Form } from 'react-bootstrap';

type SearchBoxProps = {
    placeholder?: string;
    bookTitle: string;
    onChange: (event: ChangeEvent<HTMLInputElement>) => void
}

const SearchBox = ({ placeholder, bookTitle, onChange }: SearchBoxProps) => (
  <InputGroup>
    <InputGroup.Text>
      <i className="fas fa-search"></i> {/* Font Awesome search icon */}
    </InputGroup.Text>
    <Form.Control
      type="text"
      placeholder={placeholder ?? "Book title..."}
      value={bookTitle}
      onChange={onChange}
    />
  </InputGroup>
);

export default SearchBox;
