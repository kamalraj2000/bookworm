import './App.css';
import { Container, Row, Col } from 'react-bootstrap';
import BookTable from './components/BookTable';

function App() {
  return (
    <Container className="mt-5">
      <Row>
        <Col>
          <h1 className="mb-4">Book List</h1>
          <BookTable />
        </Col>
      </Row>
    </Container>
  );
}

export default App;
