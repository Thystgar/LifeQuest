import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

export default function Header() {
  return (
	<View style={styles.header}>
	  <Text style={styles.headerText}>My Header</Text>
	</View>
  );
}

const styles = StyleSheet.create({
	header: {
		height: 60,
		backgroundColor: '#f8f8f8',
		justifyContent: 'center',
		alignItems: 'center',
		borderBottomWidth: 1,
		borderBottomColor: '#e8e8e8',
	  },
	  headerText: {
		fontSize: 20,
		fontWeight: 'bold',
	  },
});