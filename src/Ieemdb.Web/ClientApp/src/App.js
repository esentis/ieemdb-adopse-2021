import React, { Component } from 'react';
import {Container,Row} from 'react-bootstrap';

import LeftSide from './components/LeftSide'
import RightSide from './components/RightSide'

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Container fluid>
            <Row>
              <LeftSide/>
              <RightSide/>
            </Row>
        </Container>
    );
  }
}
